using Microsoft.Extensions.Logging;
using Serilog;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Dms.Integration.Infrastructure.Services;


public class ServiceCaller
{
    private readonly ILogger<ServiceCaller> _logger;

    public ServiceCaller(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ServiceCaller>();
    }

    public virtual void Call<TServiceClient>(string url, Action<TServiceClient> action, bool useCustomBinding = false, object requestForLogging = null)
    {
        Binding binding = null;

        if (useCustomBinding)
        {
            binding = getCustomBinding();
        }
        else
        {
            binding = GetBasicBinding(url);
        }

        using (var factory = new ChannelFactory<TServiceClient>(binding, new EndpointAddress(url)))
        {
            var stopwatch = new Stopwatch();
            var proxy = factory.CreateChannel();

            try
            {
                stopwatch.Start();

                action(proxy);

                stopwatch.Stop();
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                //throw CatchException(url, ex, action.Method, stopwatch.ElapsedMilliseconds, requestForLogging);
            }
            finally
            {
               // _logger.LogInformation(EventIdConstants.ServiceCallerEventId, "{Url} call lasted {ElapsedMilliseconds} milliseconds for {Action}.", url, stopwatch.ElapsedMilliseconds, cleanupMethodName(action.Method.Name));
                disposeChannel(proxy);
            }
        }
    }

    public virtual async Task<TServiceResponse> CallAsync<TServiceClient, TServiceResponse>(string url, Func<TServiceClient, Task<TServiceResponse>> action, bool useCustomBinding = false, object requestForLogging = null, string useProxyServer = null, bool ignoreSslValidation = false, string userName = null, string password = null, bool useBasicCredential = false)
    {
        Binding binding = null;

        if (useCustomBinding)
        {
            binding = getCustomBinding();
        }
        else
        {
            binding = GetBasicBinding(url, useProxyServer, useBasicCredential);
        }

        using (var factory = new ChannelFactory<TServiceClient>(binding, new EndpointAddress(url)))
        {
            if (ignoreSslValidation)
            {
                factory.Credentials.ServiceCertificate.SslCertificateAuthentication = new System.ServiceModel.Security.X509ServiceCertificateAuthentication()
                {
                    CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None,
                    RevocationMode = X509RevocationMode.NoCheck
                };
            }
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                factory.Credentials.UserName.UserName = userName;
                factory.Credentials.UserName.Password = password;
            }
            var stopwatch = new Stopwatch();
            var proxy = factory.CreateChannel();


            try
            {
                stopwatch.Start();

                var result = await action(proxy);

                stopwatch.Stop();


                return result;
            }
            finally
            {
               // _logger.LogInformation(EventIdConstants.ServiceCallerEventId, "{Url} call lasted {ElapsedMilliseconds} milliseconds for {Action}.", url, stopwatch.ElapsedMilliseconds, cleanupMethodName(action.Method.Name));
                disposeChannel(proxy);
            }
        }
    }

    public virtual TServiceResponse Call<TServiceClient, TServiceResponse>(string url, Func<TServiceClient, TServiceResponse> action, bool useCustomBinding = false, object requestForLogging = null, string useProxyServer = "")
    {
        Binding binding = null;

        if (useCustomBinding)
        {
            binding = getCustomBinding();
        }
        else
        {
            binding = GetBasicBinding(url, useProxyServer);
        }

        using (var factory = new ChannelFactory<TServiceClient>(binding, new EndpointAddress(url)))
        {
            var stopwatch = new Stopwatch();
            var proxy = factory.CreateChannel();

            try
            {
                stopwatch.Start();

                var result = action(proxy);

                stopwatch.Stop();

                return result;
            }
            finally
            {
                //_logger.LogInformation(EventIdConstants.ServiceCallerEventId, "{Url} call lasted {ElapsedMilliseconds} milliseconds for {Action}.", url, stopwatch.ElapsedMilliseconds, cleanupMethodName(action.Method.Name));
                disposeChannel(proxy);
            }
        }
    }

    public static BasicHttpBinding GetBasicBinding(string url, string useProxyServer = null, bool useBasicCredential = false)
    {
        var securityMode = BasicHttpSecurityMode.None;
        if (url.StartsWith("https://"))
        {
            securityMode = BasicHttpSecurityMode.Transport;
        }
        var binding = new BasicHttpBinding(securityMode)
        {
            MaxReceivedMessageSize = 2147483647,
            MaxBufferSize = 2147483647,
            OpenTimeout = TimeSpan.FromMinutes(10),
            CloseTimeout = TimeSpan.FromMinutes(10),
            ReceiveTimeout = TimeSpan.FromMinutes(20),
            SendTimeout = TimeSpan.FromMinutes(10),
            ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max,
        };

        if (!string.IsNullOrEmpty(useProxyServer))
        {
            binding.ProxyAddress = new Uri(useProxyServer);
            binding.UseDefaultWebProxy = false;
        }

        if (useBasicCredential)
        {
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
        }
        return binding;
    }


    private CustomBinding getCustomBinding()
    {
        HttpsTransportBindingElement httpsTransportBindingElement = new HttpsTransportBindingElement
        {
            MaxReceivedMessageSize = 65536000,
            MaxBufferSize = 65536000
        };

        TextMessageEncodingBindingElement textmessageBindingElement = new TextMessageEncodingBindingElement
        {
            MessageVersion = MessageVersion.Soap12WSAddressing10
        };

        CustomBinding customBinding = null;
        customBinding = new CustomBinding(textmessageBindingElement, httpsTransportBindingElement)
        {
            OpenTimeout = TimeSpan.FromMinutes(10),
            CloseTimeout = TimeSpan.FromMinutes(10),
            ReceiveTimeout = TimeSpan.FromMinutes(20),
            SendTimeout = TimeSpan.FromMinutes(10)
        };
        customBinding.CreateBindingElements();

        return customBinding;
    }


    private void disposeChannel<TServiceClient>(TServiceClient proxy)
    {
        var channel = proxy as IClientChannel;
        try
        {
            if (channel != null)
            {
                channel.Dispose();
            }
        }
        catch (Exception)
        {
           // _logger.LogError(EventIdConstants.ServiceCallerEventId, "Could not dispose proxy object.");
        }
    }
}

