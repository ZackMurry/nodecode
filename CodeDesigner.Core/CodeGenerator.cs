using System.Runtime.InteropServices;
using System.Text;
using CodeDesigner.Core.ast;

namespace CodeDesigner.Core
{
    
    using LLVMSharp;
    
    public class CodeGenerator
    {
        // todo: support mutation operators (through binary operator UI node, but not through binary operator compiler node)
        // todo: support logical not
        // todo: better variable scopes
        
        public static List<ErrorDescription> Run(List<ASTNode> ast)
        {
            LLVM.InitializeCore(LLVM.GetGlobalPassRegistry());
            LLVM.InitializeX86AsmPrinter();
            LLVM.InitializeX86AsmParser();
            LLVM.InitializeX86Disassembler();
            LLVM.InitializeX86Target();
            LLVM.InitializeX86TargetInfo();
            LLVM.InitializeX86TargetMC();

            var context = LLVM.ContextCreate();
            var module = LLVM.ModuleCreateWithNameInContext("program", context);
            var builder = LLVM.CreateBuilderInContext(context);

            var data = new CodegenData(builder, context, null, module, "default");
            
            var analysisManager = new AnalysisManager();
            analysisManager.AddAnalyzer(new PrototypeAnalyzer());
            analysisManager.AddAnalyzer(new GenericAnalyzer());
            analysisManager.RunAnalysis(ast);
            analysisManager.ClearAnalyzers();
            analysisManager.AddAnalyzer(new ClassAnalyzer(data));
            analysisManager.RunAnalysis(ast);
            
            // ast.Insert(0, new ASTFunctionDefinition("main", new List<ASTVariableDefinition>(), new List<ASTNode>(), new VariableType(PrimitiveVariableType.VOID)));
            foreach (var node in ast)
            {
                node.Codegen(data);
            }
            
            LLVM.DumpModule(module);
            if (data.Errors.Count != 0)
            {
                return data.Errors;
            }
            
            if (LLVM.VerifyModule(module, LLVMVerifierFailureAction.LLVMPrintMessageAction, out var error).Value != 0)
            {
                data.Errors.Add(new ErrorDescription("Failed to validate module: " + error));
                return data.Errors;
            }

            if (LLVM.WriteBitcodeToFile(module, "./output.bc") != 0)
            {
                data.Errors.Add(new ErrorDescription("Failed to write bitcode to file!"));
                return data.Errors;
            }

            var triple = Marshal.PtrToStringAnsi(LLVM.GetDefaultTargetTriple()) ?? throw new InvalidOperationException();
            Console.WriteLine("triple: " + triple);

            if (LLVM.GetTargetFromTriple(triple, out var target, out error).Value != 0)
            {
                data.Errors.Add(new ErrorDescription("Error: failed to get target from triple: " + error));
                return data.Errors;
            }

            var cpu = "generic";
            var cpuFeatures = "";
            var tm = LLVM.CreateTargetMachine(target, triple, cpu,
                cpuFeatures, LLVMCodeGenOptLevel.LLVMCodeGenLevelNone, LLVMRelocMode.LLVMRelocDefault,
                LLVMCodeModel.LLVMCodeModelDefault);
            if (LLVM.TargetMachineEmitToFile(tm, module, Marshal.StringToHGlobalAnsi("./output.o"), LLVMCodeGenFileType.LLVMObjectFile,
                    out error).Value != 0)
            {
                data.Errors.Add(new ErrorDescription("Error: failed to emit relocatable object file: " + error));
                return data.Errors;
            }
            LLVM.PrintModuleToFile(module, "./output.ir", out error);
            
            LLVM.DisposeBuilder(builder);
            LLVM.DisposeModule(module);
            LLVM.ContextDispose(context);
            return data.Errors;
        }
    }
}