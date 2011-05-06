using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Helpers {
	public class PaginatedList<T> : List<T> {

		public int PageIndex { get; private set; }
		public int PageSize { get; private set; }
		public int TotalCount { get; private set; }
		public int TotalPages { get; private set; }

		public PaginatedList(IQueryable<T> source, int pageIndex, int pageSize) {
			PageIndex = pageIndex-1;
			PageSize = pageSize;
			TotalCount = source.Count();
			TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

			this.AddRange(source.Skip(PageIndex * PageSize).Take(PageSize));
		}
	}
}