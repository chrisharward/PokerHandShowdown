using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHandShowdown
{
    public class Tokenizer
    {
        public Tokenizer(TextReader reader)
        {
            reader_ = reader;
            tokens_ = new Queue<string>();
        }

        /// <summary>
        /// Gets the next token in the fifo queue, refreshes queue if queue is empty.
        /// </summary>
        public string NextToken()
        {
            Refresh();
            if (tokens_.Count == 0)
            {
                return null;
            }
            else
            {
                return tokens_.Dequeue();
            }
        }

        /// <summary>
        /// Reads the current line and splits into a fifo queue if the queue is empty.
        /// </summary>
        private void Refresh()
        {
            if (tokens_.Count == 0)
            {
                var line = reader_.ReadLine();
                if (line == null)
                {
                    return;
                }
                tokens_ = new Queue<string>(line.Split((char[])null,
                     StringSplitOptions.RemoveEmptyEntries));
            }
        }

        private Queue<string> tokens_;
        private TextReader reader_;
    }
}
