using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Cross.Util.Extensions
{
    public static class ObjectExtension
    {
        public static List<string> GetProperties<T>(this Expression<Func<T, object>>[] action) => action.Select(NameOf).ToList();

        public static string NameOf<T>(Expression<Func<T, object>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            MemberExpression property = null;

            if (propertyExpression.Body.NodeType == ExpressionType.Convert)
            {
                var convert = propertyExpression.Body as UnaryExpression;

                if (convert != null)
                {
                    property = convert.Operand as MemberExpression;
                }
            }

            if (property == null)
            {
                property = propertyExpression.Body as MemberExpression;
            }

            if (property == null)
            {
                throw new Exception("propertyExpression cannot be null and should be passed in the format x => x.PropertyName");
            }

            return property.Member.Name;
        }
    }
}