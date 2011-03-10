using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;

namespace DeepBlue.Helpers {
	public static class QueryableExtension {
		public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool asc) {
			var type = typeof(T);
			string methodName = asc ? "OrderBy" : "OrderByDescending";
			var property = type.GetProperty(propertyName);
			var parameter = Expression.Parameter(type, "p");
			var propertyAccess = Expression.MakeMemberAccess(parameter, property);
			var orderByExp = Expression.Lambda(propertyAccess, parameter);
			MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
			return source.Provider.CreateQuery<T>(resultExp);
		}
	}
}