
using System;

namespace SimpleBlockchain
{
    public partial class Program
    {
        #region TestBlockMethods() Implementation 

        private static Block<string> CreateGenesisBlock()
        {
            var data = "Dip, dip, dip.";

            var firstBlock = Block<string>.CreateGenesisBlock(data);

            return firstBlock;
        }

        private static Block<string> CreateBlock(Block<string> previousBlock)
        {
            Block<string> newBlock = null;

            if (previousBlock != null)
            {
                var index = previousBlock.Index;
                var previousHash = previousBlock.Hash;
                var data = "My little ship";

                newBlock = new Block<string>(index, previousHash, data);
            }

            return newBlock;
        }

        private static void ValidateSecondBlock(Block<string> genesisBlock,
                                                Block<string> secondBlock)
        {
            var validationError = string.Empty;
            var isSecondBlockValid = secondBlock.IsValid(genesisBlock, out validationError);
            
            if (isSecondBlockValid)
            {
                Console.Out.WriteLine("Second Block is Valid.");
            }

            else
            {
                var invalidBlockMessage =
                $"ERROR: Second Block is Invalid - Cause: {validationError}";

                Console.Out.WriteLine(invalidBlockMessage);
            }
        }

        #endregion // TestBlockMethods() Implementation 
    }
}