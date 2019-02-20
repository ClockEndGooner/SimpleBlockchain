
using System;
using System.Linq;

namespace SimpleBlockchain
{
    public partial class Program
    {
        #region TestBlockcainMethods() Constant Definitions         

        private static int ElementNotFound = -1;

        #endregion // TestBlockcainMethods() Constant Definitions

        #region TestBlockcainMethods() Implementation         

        private static string[] GetSongLyrics()
        {
            string[] philospohersSongLyrics = 
            {
                "Immanuel Kant was a real pissant",
                "who was very rarely stable.",
                "Heidegger, Heidegger was a boozy beggar",
                "who could think you under the table.",
                "David Hume could out-consume",
                "Wilhelm Freidrich Hegel.",
                "And Wittgenstein was a beery swine",
                "who was just as schloshed as Schlegel.",
                "There's nothing Nietzsche couldn't teach ya",
                "'bout the raising of the wrist,",                
                "Socrates, himself, was permanently pissed."
            };

            return philospohersSongLyrics;
        }

        private static Blockchain<string> InitializeBlockChain()
        {
            string[] philospohersSongLyrics = GetSongLyrics();

            var philosopherSong = new Blockchain<string>();

            Console.Out.WriteLine();

            foreach (var lyric in philospohersSongLyrics)
            {
                philosopherSong.Add(lyric);

                var trace = string.Empty;

                if (philosopherSong.Contains(lyric))
                {
                    trace = $"    The Philosophers Song contains the lyric '{lyric}'";
                }           

                else
                {
                    trace = $"    The Philosophers Song does not contain the lyric '{lyric}'";
                }     

                Console.Out.WriteLine(trace);
            }

            Console.Out.WriteLine();

            return philosopherSong;
        }

        private static void TraceBlockchain(Blockchain<string> philosopherSong)
        {
            Console.WriteLine();

            var trace =
            $"{philosopherSong.GetType().Name} philosopherSong " +
            $"contains {philosopherSong.Count.ToString()} Blocks.";

            Console.Out.WriteLine(trace);

            Console.Out.WriteLine("The Lyrics in the Blockchain:");

            foreach (var lyricBlock in philosopherSong)
            {
                Console.Out.WriteLine(lyricBlock.ToString());
            }

            Console.WriteLine();
        }

        private static void ReverseTraceBlockchain(Blockchain<string> philosopherSong)
        {
            Console.Out.WriteLine();
            
            var trace =
            "The data contnents of the Blockchain in reverse order:";

            Console.Out.WriteLine(trace);

            int[] indexes = 
            Enumerable.Range(0, philosopherSong.Count).OrderByDescending(i => i).ToArray();

            foreach (var i in indexes)
            {
                var lyricBlock = philosopherSong.ElementAt(i);

                trace = 
                $"    Lyric Line [{i.ToString()}] - '{lyricBlock.Data}'";

                Console.Out.WriteLine(trace);
            }
        }

        private static int[] GetRandomizedIndexes(string[] lyrics)
        {
            var random = new Random();

            int[] indexes = Enumerable.Range(0, 11)
                                      .Select(i => new {i, r = random.Next()})
                                      .OrderBy(i => i.r)
                                      .Select(i => i.i)
                                      .ToArray();

            return indexes;
        }

        private static void TestIndexOfAndElementAt(Blockchain<string> philosopherSong)
        {
            Console.Out.WriteLine();

            string[] philospohersSongLyrics = GetSongLyrics();

            int[] randomizedIndexes = 
            GetRandomizedIndexes(philospohersSongLyrics);

            foreach (var i in randomizedIndexes)
            {
                var lyricSought = philospohersSongLyrics[i];

                var lyricIndex = philosopherSong.IndexOf(lyricSought);

                if (lyricIndex > ElementNotFound)
                {
                    var lyricBlock = philosopherSong.ElementAt(lyricIndex);

                    var trace =
                    $"    The lyric '{lyricBlock.Data}' was found " +
                    $"at offset: [{lyricIndex.ToString()}]";

                    Console.Out.WriteLine(trace);
                }
            }

            Console.Out.WriteLine();
        }        

        #endregion // TestBlockcainMethods() Implementation 
    }
}
