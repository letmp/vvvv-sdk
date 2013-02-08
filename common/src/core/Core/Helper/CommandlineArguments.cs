﻿using System;
using System.IO;
using CommandLine;
using CommandLine.Text;
using System.Text;

namespace VVVV.Core
{
    public class CommandLineArguments
    {
        //a file to open on startup
        [Option("o", "open", Required = false, HelpText = "File to open on startup")]
        public string InputFile = null;

        [Option("e", "throwexceptions", Required = false, HelpText = "If true, vvvv will stop on exceptions")]
        public bool ThrowExceptions = false;

        [Option("f", "throwfrontendexceptions", Required = false, HelpText = "If true, vvvv will stop on frontend compiler exceptions")]
        public bool ThrowFrontEndExceptions = false;

        [Option("b", "throwbackendexceptions", Required = false, HelpText = "If true, vvvv will stop on backend compiler exceptions")]
        public bool ThrowBackEndExceptions = false;

        [Option("l", "local", Required = false, HelpText = "If true, vvvv will not try to communicate with a runtime")]
        public bool Local = false;

        [OptionArray("c", "client", Required = false, HelpText = "Space separated list of the IP's of the clients to connect to")]
        public string[] ClientIPs;

        [Option("p", "port", Required = false, HelpText = "Sets the port in which the HDE communicates with the clients, default is 3344")]
        public int Port = 3344;

        [Option("s", "startruntime", Required = false, HelpText = "If set, the Runtime.exe at the given path will be started")]
        public string RuntimePath = @"..\..\..\Runtime\bin\Debug\VClient.exe";

        //help string
        [HelpOption(HelpText = "Dispaly this help screen")]
        public string GetUsage()
        {

            var help = new HelpText("Could not parse commandline arguments, use the following options:");
            help.AdditionalNewLineAfterOption = true;
            help.AddOptions(this);

            return help;
        }

        //parse the commandline and args.txt
        public void Parse()
        {
            var parser = new CommandLineParser();

            try //parse Args.txt
            {
                var streamReader = new StreamReader(Path.Combine(Environment.CurrentDirectory, "Args.txt"));
                var text = streamReader.ReadToEnd();
                var args = text.Split('\n');
                streamReader.Close();
                if (!parser.ParseArguments(args, this, Console.Out))
                {
                    Console.WriteLine("Args.txt has syntax error(s):");
                    foreach (var arg in args)
                    {
                        Console.WriteLine(arg);
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not parse Args.txt: " + e.Message);
            }

            //parse commandline
            if (!parser.ParseArguments(Environment.GetCommandLineArgs(), this, Console.Out))
            {
                Console.WriteLine("Console command line has syntax error(s):");
                foreach (var arg in Environment.GetCommandLineArgs())
                {
                    Console.WriteLine(arg);
                }
                Console.WriteLine();
            }
        }

    }
}
