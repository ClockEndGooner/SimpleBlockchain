
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace SimpleBlockchain
{
    ///////////////////////////////////////////////////////////////////////////////////
    //
    // A simple Blockchain node or Block definition based on 
    // Lauri Hartikka's blog post on Medium, "A blockchain in 200 lines
    // of code" (in Javascript) at:
    //
    // https://medium.com/@lhartikk/a-blockchain-in-200-lines-of-code-963cc1cc0e54
    //
    ///////////////////////////////////////////////////////////////////////////////////
    public class Block<T>
    {
        #region Block Class Constant Definitions

        private const int StringsMatch = 0;

        #endregion // Block Class Constant Definitions

        #region Block Class Data Members

        public int Index { get; private set; }
        public string PreviousHash { get; private set; }
        public DateTime UTCTimeStamp { get; private set; }
        public T Data { get; private set; }
        public string Hash { get; private set; }

        #endregion // Block Class Data Members

        #region Block Class Constructor

        public Block(int index, string previousHash, T data)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index",
                "ERROR: Block Index must be greater than zero.");
            }

            Index = index + 1;

            if (string.IsNullOrEmpty(previousHash))
            {
                throw new ArgumentOutOfRangeException("previousHash",
                "ERROR: Previous Hash must be a Non-Null and Non-Empty string.");
            }

            PreviousHash = previousHash;

            UTCTimeStamp = DateTime.UtcNow;

            if (data == null)
            {
                throw new ArgumentNullException("data",
                "ERROR: Block data value cannot be null.");
            }

            Data = data;

            Hash = GenerateBlockHash(Index, PreviousHash, UTCTimeStamp, Data);
        }

        #endregion Block Class Constructor

        #region Block Class Implementation

        internal static string GenerateBlockHash(int index, string previousHash, 
                                                 DateTime utcTimeStamp, T data)
        {
            var dataByteArray = ConvertDataToByteArray(data);

            var blockValue = 
            $"{index.ToString()}{previousHash}{utcTimeStamp.ToUniversalTime().ToString()}";

            var sha256Hash = new SHA256Managed();
            var blockValueBytes = Encoding.UTF8.GetBytes(blockValue);
            var blockHash = sha256Hash.ComputeHash(blockValueBytes);

            return
            BitConverter.ToString(blockHash).Replace("-", string.Empty).ToUpper();
        }

        public static Block<T> CreateGenesisBlock(T data)
        {
            var guidHash = GetSHA256ForGUID();

            return new Block<T>(0, guidHash, data);
        }

        private static string GetSHA256ForGUID()
        {
            var guid = Guid.NewGuid();
            var guidString = guid.ToString().Replace("-", string.Empty).ToUpper();
            var guidBytes = Encoding.UTF8.GetBytes(guidString);

            var sha256Hash = new SHA256Managed();
            var guidHash = sha256Hash.ComputeHash(guidBytes);

            return BitConverter.ToString(guidHash).Replace("-", string.Empty).ToUpper();
        }

        internal static byte[] ConvertDataToByteArray(object data)
        {
            byte[] dataByteArray = null;

            if (data != null)
            {
                var binaryFormatter = new BinaryFormatter();

                using (var memoryStream = new MemoryStream())
                {
                    binaryFormatter.Serialize(memoryStream, data);

                    dataByteArray = memoryStream.ToArray();
                }
            }

            return dataByteArray;
        }

        internal static bool IsValid(string previousHash, Block<T> block, 
                                     out string lastValidationError)
        {
            lastValidationError = string.Empty;

            var isValidBlock = false;

            if (!string.IsNullOrEmpty(previousHash))
            {
                if ((string.Compare(previousHash, block.PreviousHash)) == StringsMatch)
                {
                    var blockHash =
                    GenerateBlockHash(block.Index, block.PreviousHash, block.UTCTimeStamp, block.Data);

                    if (string.Compare(block.Hash, blockHash) == StringsMatch)
                    {
                        isValidBlock = true;
                    }

                    else
                    {
                        lastValidationError =
                        $"Mismatch between Block.Hash and the Generated Hash value for Block {block.Index.ToString()}";                        
                    }
                }

                else
                {
                    lastValidationError =
                    $"Mismatch between PreviousBlock.Hash and this Block.PreviousHash in Block {block.Index.ToString()}";
                }
            }

            else
            {
                lastValidationError = 
                "PreviousBlock was a Null object reference.";
            }
            
            return isValidBlock;
        }
        
        public bool IsValid(Block<T> previousBlock, 
                            out string lastValidationError)
        {
            lastValidationError = string.Empty;

            var isValidBlock = false;

            if (previousBlock != null)
            {
                isValidBlock =  IsValid(previousBlock.Hash, this, 
                                        out lastValidationError);
            }

            else
            {
                lastValidationError = 
                "PreviousBlock was a Null object reference.";
            }

            return isValidBlock;
        }

        public override string ToString()
        {
            var traceBlock = new StringBuilder();

            traceBlock.AppendFormat("{0} Contents:", this.GetType().Name);
            traceBlock.AppendLine();

            traceBlock.AppendFormat("           Index: {0}", Index.ToString());
            traceBlock.AppendLine();

            traceBlock.AppendFormat("    PreviousHash: {0}", PreviousHash);
            traceBlock.AppendLine();

            traceBlock.AppendFormat("    UTCTimeStamp: {0}", UTCTimeStamp.ToUniversalTime().ToString());
            traceBlock.AppendLine();

            traceBlock.AppendFormat("            Data: {0}", Data.ToString());
            traceBlock.AppendLine();

            traceBlock.AppendFormat("            Hash: {0}", Hash);
            traceBlock.AppendLine();

            return traceBlock.ToString();
        }

        #endregion Block Class Implementation
    }
}

