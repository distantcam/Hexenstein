using System.Linq;
using Autofac;
using Autofac.Core;

namespace Hexenstein.Framework
{
    public interface IAttachment
    {
        void AttachTo(object obj);
    }

    public abstract class Attachment<T> : IAttachment
    {
        protected T viewModel;

        protected abstract void OnAttach();

        void IAttachment.AttachTo(object obj)
        {
            viewModel = (T)obj;
            OnAttach();
        }
    }

    public class AutoAttachmentModule : Module
    {
        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Activating += Activating;
        }

        private void Activating(object sender, ActivatingEventArgs<object> e)
        {
            var vmType = e.Instance.GetType();

            if (!vmType.Name.EndsWith("ViewModel"))
                return;

            var attachments = Conventions.FindAll<AttachmentConventions>(vmType)
                .Where(t => e.Context.IsRegistered(t))
                .Select(t => (IAttachment)e.Context.Resolve(t));

            foreach (var a in attachments)
                a.AttachTo(e.Instance);
        }
    }
}