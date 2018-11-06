/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDToolbox.Data
{
    /// <summary>
    /// useful for items in list boxes, etc
    /// Links a piece of text with some data.
    /// Usefully, the text, also takes over the ToString value.
    /// eg: new Tag("one", 1).ToString()
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Tag<T>
    {
        public string Text { get; protected set; }
        public T      Data{ get; protected set; }

        public Tag(T data, string text)
        {
            Text = text;
            Data = data;
        }

        private Tag(T data)
        {
            Data = data;
            Text = data.ToString();
        }

        public override string ToString()
        {
            return Text;
        }

        public static implicit operator T(Tag<T> tag)
        {
            return tag.Data;
        }

        public static Tag<T> fromToString(T data)
        {
            return new Tag<T>(data);
        }
    }
}
