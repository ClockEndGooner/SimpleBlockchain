
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleBlockchain
{
    public class Blockchain<T> : IEnumerable
    {
        #region Blockchain<T> Class Constant Definitions

        private const int DataIndexNotFound = -1;

        #endregion // Blockchain<T> Class Constant Definitions

        #region Blockchain<T> Class Data Members

        private List<Block<T>> theBlockchain;

        #endregion // Blockchain<T> Class Data Members

        #region Blockchain<T> Class Constructor

        public Blockchain()
        {
            theBlockchain = new List<Block<T>>();
        }

        #endregion // Blockchain<T> Class Constructor

        #region Blockchain<T> IEnumerable Interface Implementation

        public IEnumerator GetEnumerator()
        {
            return theBlockchain.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion // Blockchain<T> IEnumerable Interface Implementation

        #region Blockchain<T> Class Implementation

        public int Count
        {
            get
            {
                return theBlockchain.Count();
            }
        }

        public void Add(T data)
        {
            if (data != null)
            {
                Block<T> newBlock = null;
                Block<T> previousBlock = null;
                var previousHash = string.Empty;

                if (theBlockchain.Count() > 0)
                {
                    var lastIndex = theBlockchain.Count;

                    previousBlock = theBlockchain[lastIndex - 1];
                    previousHash = previousBlock.Hash;

                    var newIndex = previousBlock.Index;

                    newBlock = new Block<T>(newIndex, previousBlock.Hash, data);
                }

                else
                {
                    newBlock = Block<T>.CreateGenesisBlock(data);
                    previousHash = newBlock.PreviousHash;
                }

                var validationError = string.Empty;
        
                if (Block<T>.IsValid(previousHash, newBlock, out validationError))
                {
                    theBlockchain.Add(newBlock);
                }

                else
                {
                    throw new InvalidOperationException(validationError);
                }
            }
        }

        public bool Contains(T data)
        {
            if (theBlockchain.Count > 0)
            {
                    var containingBlock = 
                    theBlockchain.FirstOrDefault(b => b.Data.Equals(data));

                    return (containingBlock != null);
            }

            else
            {
                return false;
            }
        }

        public int IndexOf(T data)
        {
            var dataIndex = DataIndexNotFound;

            if (theBlockchain.Count > 0)
            {
                var block = 
                theBlockchain.FirstOrDefault(b => b.Data.Equals(data));
                
                dataIndex = (block.Index - 1);
            }

            return dataIndex;
        }

        public Block<T> ElementAt(int index)
        {
            if (index <= (theBlockchain.Count - 1))
            {
                return theBlockchain.ElementAt(index);
            }

            else
            {
                throw new IndexOutOfRangeException(
                $"ERROR: The index {index.ToString()} exceeds the number of " +
                $"Blocks in the Blockchain [{(theBlockchain.Count() - 1).ToString()}]");
            }
        }        

        #endregion Blockchain<T> Class Implementation
    }
}
