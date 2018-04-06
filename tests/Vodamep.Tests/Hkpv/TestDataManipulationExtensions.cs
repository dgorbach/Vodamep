using Vodamep.Hkpv.Model;
using System.Linq.Expressions;
using System;
using System.Reflection;

namespace Vodamep
{
    public static class TestDataManipulationExtensions
    {
        public static (Person Person, PersonalData Data) Manipulate(this (Person Person, PersonalData Data) p, Action<(Person Person, PersonalData Data)>a )
        {
            a?.Invoke(p);

            return p;
        }

        public static (Person Person, PersonalData Data) ManipulatePerson<T>(this (Person Person, PersonalData Data) p, Expression<Func<Person, T>> expression, T value)
        {
            var memberSelectorExpression = expression.Body as MemberExpression;
            if (memberSelectorExpression != null)
            {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null)
                {
                    property.SetValue(p.Person, value, null);
                }
            }

            return p;
        }

        public static (Person Person, PersonalData Data) ManipulateData<T>(this (Person Person, PersonalData Data) p, Expression<Func<PersonalData, T>> expression, T value)
        {
            var memberSelectorExpression = expression.Body as MemberExpression;
            if (memberSelectorExpression != null)
            {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null)
                {
                    property.SetValue(p.Data, value, null);
                }
            }

            return p;
        }


    }
}
