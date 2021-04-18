using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MLocati.ServicesControl
{
    public class ProgramOutput
    {
        public event EventHandler Cleared;
        public event ChunkAddedEventHandler ChunkAdded;
        public class ChunkAddedEventArgs : EventArgs
        {
            public readonly Chunk Chunk;
            public ChunkAddedEventArgs(Chunk chunk)
            {
                this.Chunk = chunk;
            }
        }
        public delegate void ChunkAddedEventHandler(object sender, ChunkAddedEventArgs e);

        public readonly List<Chunk> Chunks = new List<Chunk>();

        public enum Type
        {
            StdOut,
            StdErr,
        }

        public class Chunk
        {
            public readonly string Text;
            public readonly Type Type;
            public Chunk(string text, Type type)
            {
                this.Text = text;
                this.Type = type;
            }
        }

        public void Clear()
        {
            this.Chunks.Clear();
            if (this.Cleared != null)
            {
                this.Cleared(this, new EventArgs());
            }
        }

        public void Add(string text, Type type)
        {
            var chunk = new Chunk(text, type);
            this.Add(chunk);
            if (this.ChunkAdded != null)
            {
                this.ChunkAdded(this, new ChunkAddedEventArgs(chunk));
            }
        }

        public void Add(Chunk chunk)
        {
            this.Chunks.Add(chunk);
        }
    }
}
