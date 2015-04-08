using System;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    internal static class IEquivalencyValidationContextExtentions
    {
        internal static IEquivalencyValidationContext CreateForNestedMember(
            this IEquivalencyValidationContext context,
            SelectedMemberInfo nestedMember,
            SelectedMemberInfo matchingProperty)
        {
            object subject = nestedMember.GetValue(context.Subject, null);
            object expectation = matchingProperty.GetValue(context.Expectation, null);

            return CreateNested(
                context,
                nestedMember,
                subject,
                expectation,
                "member ",
                nestedMember.Name,
                ".",
                nestedMember.MemberType);
        }

        internal static IEquivalencyValidationContext CreateForCollectionItem<T>(
            this IEquivalencyValidationContext context,
            int index,
            T subject,
            object expectation)
        {
            return CreateNested(
                context,
                context.SelectedMemberInfo,
                subject,
                expectation,
                "item",
                "[" + index + "]",
                String.Empty,
                typeof (T));
        }

        internal static IEquivalencyValidationContext CreateForDictionaryItem<TKey, TValue>(
            this IEquivalencyValidationContext context,
            TKey key,
            TValue subject,
            object expectation)
        {
            return CreateNested(
                context,
                context.SelectedMemberInfo,
                subject,
                expectation,
                "pair",
                "[" + key + "]",
                String.Empty,
                typeof (TValue));
        }

        internal static IEquivalencyValidationContext CreateWithDifferentSubject(
            this IEquivalencyValidationContext context,
            object subject,
            Type compileTimeType)
        {
            return new EquivalencyValidationContext
            {
                CompileTimeType = compileTimeType,
                Expectation = context.Expectation,
                SelectedMemberDescription = context.SelectedMemberDescription,
                SelectedMemberInfo = context.SelectedMemberInfo,
                SelectedMemberPath = context.SelectedMemberPath,
                Reason = context.Reason,
                ReasonArgs = context.ReasonArgs,
                Subject = subject
            };
        }

        private static IEquivalencyValidationContext CreateNested(
            this IEquivalencyValidationContext context,
            SelectedMemberInfo subjectProperty,
            object subject,
            object expectation,
            string memberType,
            string memberDescription,
            string separator,
            Type compileTimeType)
        {
            string propertyPath = context.IsRoot
                ? memberType
                : context.SelectedMemberDescription + separator;

            return new EquivalencyValidationContext
            {
                SelectedMemberInfo = subjectProperty,
                Subject = subject,
                Expectation = expectation,
                SelectedMemberPath = context.SelectedMemberPath.Combine(memberDescription, separator),
                SelectedMemberDescription = propertyPath + memberDescription,
                Reason = context.Reason,
                ReasonArgs = context.ReasonArgs,
                CompileTimeType = compileTimeType
            };
        }
    }
}