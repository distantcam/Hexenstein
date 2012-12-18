using System;
using System.Collections.Generic;
using System.ComponentModel;
using Conventional;
using Conventional.Conventions;

namespace Hexenstein.Framework
{
    internal class AttachmentConventions : Convention
    {
        public AttachmentConventions()
        {
            Must
                .HaveNamespaceStartsWith("Hexenstein.UI")
                .HaveNameEndWith("Attachment");

            Should
                .BeAConcreteClass()
                .Implement<IAttachment>();

            BaseName = t => t.Name.Substring(0, t.Name.Length - 10);

            Variants
                .StartWithBaseName();
        }
    }

    internal class ViewModelConventions : Convention
    {
        public ViewModelConventions()
        {
            Must
                .HaveNamespaceStartsWith("Hexenstein.UI")
                .HaveNameEndWith("ViewModel");

            Should
                .BeAConcreteClass()
                .Implement<INotifyPropertyChanged>();

            BaseName = t => t.Name.Substring(0, t.Name.Length - 9);

            Variants
                .StartWithBaseName();
        }
    }

    internal class ViewConventions : Convention
    {
        public ViewConventions()
        {
            Must
                .HaveNamespaceStartsWith("Hexenstein.UI")
                .HaveNameEndWith("View");

            Should
                .BeAConcreteClass();

            BaseName = t => t.Name.Substring(0, t.Name.Length - 4);

            Variants
                .StartWithBaseName();
        }
    }

    internal class ServiceConventions : Convention
    {
        public ServiceConventions()
        {
            Must
                .HaveNamespace("Hexenstein.Framework.Services")
                .BeAConcreteClass();
        }
    }

    internal static class Conventions
    {
        private static readonly IConventionManager conventions;

        static Conventions()
        {
            var builder = new ConventionBuilder();

            //foreach (var assm in AssemblySource.Instance)
            builder.ScanThisAssembly()
                .For<AttachmentConventions>()
                .For<ViewModelConventions>()
                .For<ViewConventions>()
                .For<ServiceConventions>()
                ;

            conventions = builder.Build();
        }

        public static IEnumerable<Type> FindAll<TConvention>() where TConvention : Convention
        {
            return conventions.FindAll<TConvention>();
        }

        public static IEnumerable<Type> FindAll<TConvention>(Type other) where TConvention : Convention
        {
            return conventions.FindAll<TConvention>(other);
        }

        public static void Verify(bool logOnly = false)
        {
            conventions.Verify(logOnly);
        }
    }
}