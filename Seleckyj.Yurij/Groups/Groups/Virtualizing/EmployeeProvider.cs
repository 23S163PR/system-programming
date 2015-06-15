using System.Collections.Generic;
using System.Diagnostics;

namespace Groups.Virtualizing
{
    /// <summary>
    /// Demo implementation of IItemsProvider returning dummy customer items after
    /// a pause to simulate network/disk latency.
    /// </summary>
    public class EmployeeProvider : IItemsProvider<Employee>
    {
        private readonly int _count;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeProvider"/> class.
        /// </summary>
        /// <param name="count">The count.</param>
        public EmployeeProvider(int count)
        {
            _count = count;
        }

        /// <summary>
        /// Fetches the total number of items available.
        /// </summary>
        /// <returns></returns>
        public int FetchCount()
        {
            return _count;
        }

        /// <summary>
        /// Fetches a range of items.
        /// </summary>
        /// <param name="employees"></param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The number of items to fetch.</param>
        /// <returns></returns>
        public IList<Employee> FetchRange(List<Employee> employees, int startIndex, int count)
        {
            Trace.WriteLine("FetchRange: " + startIndex + "," + count);
            var list = new List<Employee>();
            for (var i = startIndex; i < startIndex + count && i < _count; i++)
            {
                list.Add(employees[i]);
            }
            return list;
        }
    }
}
