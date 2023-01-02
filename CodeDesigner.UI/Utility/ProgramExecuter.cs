using System.Diagnostics;
using CodeDesigner.UI.Node.Blocks;
using CodeDesigner.UI.Node.Canvas;

namespace CodeRunner.UI.Utility;

public class ProgramExecuter
{
    public static string ExecuteProgram()
    {
        Process p = new Process();
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.FileName = "cmd.exe";
        var projectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
        Console.WriteLine(projectDir);
        p.StartInfo.Arguments = "/c gcc " + projectDir + Path.DirectorySeparatorChar + "linker.cpp " + projectDir + Path.DirectorySeparatorChar + "CodeDesigner.UI" + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "debug" + Path.DirectorySeparatorChar + "net6.0-windows" + Path.DirectorySeparatorChar + "output.o -static";
        Console.WriteLine(p.StartInfo.Arguments);
        p.Start();
        var strOutput = p.StandardOutput.ReadToEnd();
        Console.WriteLine(strOutput);
        p.WaitForExit();
        Console.WriteLine("executed ld");
        Process program = new Process();
        program.StartInfo.UseShellExecute = false;
        program.StartInfo.RedirectStandardOutput = true;
        program.StartInfo.FileName = "a.exe";
        program.Start();
        
        Console.WriteLine("Started a.exe");
        var programOutput = program.StandardOutput.ReadToEnd();
        Console.WriteLine(programOutput);
        program.WaitForExit();
        return programOutput;
    }
}