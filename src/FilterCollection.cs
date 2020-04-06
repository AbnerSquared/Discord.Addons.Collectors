using System;
using System.Collections;
using System.Collections.Generic;

namespace Discord.Addons.Collectors
{
    /// <summary>
    /// Represents a collection of <see cref="FilterResult"/> values received from a <see cref="MessageCollector"/>.
    /// </summary>
    public class FilterCollection : IEnumerable<FilterResult>
    {
        /// <summary>
        /// The raw collection of matches.
        /// </summary>
        public List<FilterResult> Matches { get; set; } = new List<FilterResult>();

        public IEnumerator<FilterResult> GetEnumerator()
            => Matches.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => Matches.GetEnumerator();

        /// <summary>
        /// Returns the number of matches collected.
        /// </summary>
        public int Count => Matches.Count;

        /// <summary>
        /// Adds a <see cref="FilterResult"/> to the <see cref="FilterCollection"/>.
        /// </summary>
        internal void Add(FilterResult match)
        {
            Matches.Add(match);
        }

        /// <summary>
        /// Converts all of the matches into the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="TValue">The enclosing <see cref="Type"/> that the <see cref="FilterResult"/> will convert to.</typeparam>
        /// <param name="converter">The method used to convert the <see cref="FilterResult"/>.</param>
        public List<TValue> Convert<TValue>(Func<FilterResult, TValue> converter)
        {
            var results = new List<TValue>();

            foreach (FilterResult match in Matches)
            {
                results.Add(match.Convert(converter));
            }

            return results;
        }

        public FilterResult this[int i]
        {
            get => Matches[i];
            set => Matches[i] = value;
        }
    }
}
