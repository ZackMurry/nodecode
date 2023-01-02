using LLVMSharp;

namespace CodeDesigner.Core.ast;

using Core;

public class ASTBinaryExpression : ASTNode
{
    public BinaryOperator Op;
    public ASTNode Lhs;
    public ASTNode Rhs;

    public ASTBinaryExpression(BinaryOperator op, ASTNode lhs, ASTNode rhs)
    {
        Op = op;
        Lhs = lhs;
        Rhs = rhs;
    }
    
    public override LLVMValueRef? Codegen(CodegenData data)
    {
        if (!data.Func.HasValue)
        {
            data.Errors.Add(new ErrorDescription("Error: binary expressions need to be inside a function", id));
            return null;
        }
        if (Op is BinaryOperator.AND or BinaryOperator.OR) {
            var lhsValue = Lhs.Codegen(data);
            if (lhsValue == null) return null;
            var ifBlock = LLVM.AppendBasicBlockInContext(data.Context, data.Func.Value, "condif");
            var mergeBlock = LLVM.AppendBasicBlockInContext(data.Context, data.Func.Value, "mergeif");
            var resultAlloca = LLVM.BuildAlloca(data.Builder, LLVM.Int1TypeInContext(data.Context), "condtmp");
            LLVM.BuildStore(data.Builder, lhsValue ?? throw new Exception(), resultAlloca);
            if (Op == BinaryOperator.AND) {
                LLVM.BuildCondBr(data.Builder, lhsValue ?? throw new Exception(), ifBlock, mergeBlock);
            } else {
                LLVM.BuildCondBr(data.Builder, lhsValue ?? throw new Exception(), mergeBlock, ifBlock);
            }
            LLVM.PositionBuilderAtEnd(data.Builder, ifBlock);
            var rhsValue = Rhs.Codegen(data);
            if (rhsValue == null) return null;
            LLVM.BuildStore(data.Builder, rhsValue ?? throw new Exception(), resultAlloca);
            LLVM.BuildBr(data.Builder, mergeBlock);
            LLVM.PositionBuilderAtEnd(data.Builder, mergeBlock);
            return LLVM.BuildLoad(data.Builder, resultAlloca, "");
        }
        var l = Lhs.Codegen(data);
        var r = Rhs.Codegen(data);
        if (l == null || r == null) return null;
        var typeKind = LLVM.GetTypeKind(LLVM.TypeOf(l ?? throw new Exception()));
        switch (Op) {
            case BinaryOperator.PLUS:
                if (typeKind == LLVMTypeKind.LLVMIntegerTypeKind) {
                    var a = LLVM.BuildAdd(data.Builder, l ?? throw new Exception(), r ?? throw new Exception(), "addtmp");
                    return a;
                }
                if (typeKind == LLVMTypeKind.LLVMDoubleTypeKind) {
                    return LLVM.BuildFAdd(data.Builder, l ?? throw new Exception(), r ?? throw new Exception(), "addtmp");
                }
                data.Errors.Add(new ErrorDescription("Error: unknown add type", id));
                return null;
            case BinaryOperator.MINUS:
                if (typeKind == LLVMTypeKind.LLVMIntegerTypeKind) {
                    return LLVM.BuildSub(data.Builder, l ?? throw new Exception(), r ?? throw new Exception(), "subtmp");
                }
                if (typeKind == LLVMTypeKind.LLVMDoubleTypeKind) {
                    return LLVM.BuildFSub(data.Builder, l ?? throw new Exception(), r ?? throw new Exception(), "subtmp");
                }
                data.Errors.Add(new ErrorDescription("Error: unknown subtract type", id));
                return null;
            case BinaryOperator.TIMES:
                if (typeKind == LLVMTypeKind.LLVMIntegerTypeKind) {
                    return LLVM.BuildMul(data.Builder, l ?? throw new Exception(), r ?? throw new Exception(), "multmp");
                }
                if (typeKind == LLVMTypeKind.LLVMDoubleTypeKind) {
                    return LLVM.BuildFMul(data.Builder, l ?? throw new Exception(), r ?? throw new Exception(), "multmp");
                }
                data.Errors.Add(new ErrorDescription("Error: unknown multiply type", id));
                return null;
            case BinaryOperator.DIVIDE:
                if (typeKind == LLVMTypeKind.LLVMIntegerTypeKind) {
                    return LLVM.BuildSDiv(data.Builder, l ?? throw new Exception(), r ?? throw new Exception(), "divtmp");
                }
                if (typeKind == LLVMTypeKind.LLVMDoubleTypeKind) {
                    return LLVM.BuildFDiv(data.Builder, l ?? throw new Exception(), r ?? throw new Exception(), "divtmp");
                }
                data.Errors.Add(new ErrorDescription("Error: unknown divide type", id));
                return null;
            case BinaryOperator.LT:
                if (typeKind == LLVMTypeKind.LLVMIntegerTypeKind) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntSLT, l ?? throw new Exception(), r ?? throw new Exception(), "cmptmp");
                }
                if (typeKind == LLVMTypeKind.LLVMDoubleTypeKind) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealOLT, l ?? throw new Exception(), r ?? throw new Exception(), "cmptmp");
                }
                data.Errors.Add(new ErrorDescription("Error: unknown comparison type", id));
                return null;
            case BinaryOperator.GT:
                if (typeKind == LLVMTypeKind.LLVMIntegerTypeKind) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntSGT, l ?? throw new Exception(), r ?? throw new Exception(), "cmptmp");
                }
                if (typeKind == LLVMTypeKind.LLVMDoubleTypeKind) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealOGT, l ?? throw new Exception(), r ?? throw new Exception(), "cmptmp");
                }
                data.Errors.Add(new ErrorDescription("Error: unknown comparison type", id));
                return null;
            case BinaryOperator.LE:
                if (typeKind == LLVMTypeKind.LLVMIntegerTypeKind) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntSLE, l ?? throw new Exception(), r ?? throw new Exception(), "cmptmp");
                }
                if (typeKind == LLVMTypeKind.LLVMDoubleTypeKind) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealOLT, l ?? throw new Exception(), r ?? throw new Exception(), "cmptmp");
                }
                data.Errors.Add(new ErrorDescription("Error: unknown comparison type", id));
                return null;
            case BinaryOperator.GE:
                if (typeKind == LLVMTypeKind.LLVMIntegerTypeKind) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntSGT, l ?? throw new Exception(), r ?? throw new Exception(), "cmptmp");
                }
                if (typeKind == LLVMTypeKind.LLVMDoubleTypeKind) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealOGT, l ?? throw new Exception(), r ?? throw new Exception(), "cmptmp");
                }
                data.Errors.Add(new ErrorDescription("Error: unknown comparison type", id));
                return null;
            case BinaryOperator.EQ:
                if (typeKind == LLVMTypeKind.LLVMIntegerTypeKind) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntEQ, l ?? throw new Exception(), r ?? throw new Exception(), "cmptmp");
                }
                if (typeKind == LLVMTypeKind.LLVMDoubleTypeKind) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealOEQ, l ?? throw new Exception(), r ?? throw new Exception(), "cmptmp");
                }
                data.Errors.Add(new ErrorDescription("Error: unknown comparison type", id));
                return null;
            case BinaryOperator.NE:
                if (typeKind == LLVMTypeKind.LLVMIntegerTypeKind) {
                    return LLVM.BuildICmp(data.Builder, LLVMIntPredicate.LLVMIntNE, l ?? throw new Exception(), r ?? throw new Exception(), "cmptmp");
                }
                if (typeKind == LLVMTypeKind.LLVMDoubleTypeKind) {
                    return LLVM.BuildFCmp(data.Builder, LLVMRealPredicate.LLVMRealONE, l ?? throw new Exception(), r ?? throw new Exception(), "cmptmp");
                }
                data.Errors.Add(new ErrorDescription("Error: unknown comparison type", id));
                return null;
            case BinaryOperator.MODULO:
                if (typeKind == LLVMTypeKind.LLVMIntegerTypeKind) {
                    return LLVM.BuildSRem(data.Builder, l ?? throw new Exception(), r ?? throw new Exception(), "modtmp");
                }
                if (typeKind == LLVMTypeKind.LLVMDoubleTypeKind) {
                    return LLVM.BuildFRem(data.Builder, l ?? throw new Exception(), r ?? throw new Exception(), "modtmp");
                }
                data.Errors.Add(new ErrorDescription("Error: unknown modulo type", id));
                return null;
            default:
                data.Errors.Add(new ErrorDescription("Error: unimplemented binary expression type", id));
                return null;
        }

    }
}