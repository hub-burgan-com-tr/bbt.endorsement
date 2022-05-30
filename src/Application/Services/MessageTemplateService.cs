using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MessageTemplateService
    {
        private MessageTemplateService(string value) { Value = value; }

        public string Value { get; private set; }

        /// <summary>
        /// Burgan
        /// </summary>
        public static MessageTemplateService BurganPyOnayEposta { get { return new MessageTemplateService("e5b6c98f-60af-4aff-aca1-c44bf6593520"); } }
        public static MessageTemplateService BurganPyOnaylanmayanEposta { get { return new MessageTemplateService("e5cfb466-3c73-4c67-94fa-f77380d735c9"); } }
        public static MessageTemplateService BurganMusteriGidenBasvuruOnay { get { return new MessageTemplateService("bcbb8a2a-bb68-46c0-8da7-19bb38f11be7"); } }
        public static MessageTemplateService BurganMusteriTalepEdilenOnaySMS { get { return new MessageTemplateService("37fedb31-f07e-4fec-93a1-5a27cbd74982"); } }
        /// <summary>
        /// Burgan On
        /// </summary>
        public static MessageTemplateService PyOnayEposta { get { return new MessageTemplateService("b346c897-cdcc-4939-bb7d-70f6ff16ade9"); } }
        public static MessageTemplateService PyOnaylanmayanEposta { get { return new MessageTemplateService("8cdebd39-b84b-4cb8-9426-e858ef4ffa8b"); } }
        public static MessageTemplateService MusteriGidenBasvuruOnay { get { return new MessageTemplateService("0a456cf9-69c9-4633-8c3a-5107e72742bd"); } }
        public static MessageTemplateService MusteriTalepEdilenOnaySMS { get { return new MessageTemplateService("87f5a560-89aa-48bb-a3aa-25e6945e1dfa"); } }

    }
}
