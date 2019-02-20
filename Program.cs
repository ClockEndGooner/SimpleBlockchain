
using System;
using System.Collections.Generic;

namespace SimpleBlockchain
{
    public partial class Program
    {
        static void Main(string[] args)
        {
            TestBlockMethods();

            TestBlockcainMethods();
        }

        private static void TestBlockMethods()
        {
            Console.Out.WriteLine("Simple BlockChain Console Application");

            var genesisBlock = CreateGenesisBlock();

            Console.Out.WriteLine(genesisBlock.ToString());

            var secondBlock = CreateBlock(genesisBlock);

            Console.Out.WriteLine(secondBlock.ToString());

            ValidateSecondBlock(genesisBlock, secondBlock);
        }

        private static void TestBlockcainMethods()
        {
            Blockchain<string> philosopherSong = InitializeBlockChain();

            TraceBlockchain(philosopherSong);

            ReverseTraceBlockchain(philosopherSong);

            TestIndexOfAndElementAt(philosopherSong);
        }
    }
}
