using System;
using System.Diagnostics;
using System.Linq;
using GisFramework.Data;
using GisFramework.Domains;
using GisFramework.Interfaces;
using GisFramework.Interfaces.Converters;
using GisFramework.Interfaces.Handlers;
using GisFramework.Interfaces.Providers;
using GisFramework.Interfaces.Services;

namespace GisFramework.CoreServices
{
	/// <summary>
	/// ������� ������ ��� �������� ����� �������������� - ��������� ���������� ���������
	/// </summary>
	/// <typeparam name="TMessageDomain">��� ��������� ���������</typeparam>
	/// <typeparam name="TGetStateResultProxy">��� ������ ������� ������� ���������� ���������</typeparam>
	/// <typeparam name="TResultProxy">��� ������ ������� ���������� ��������� ���������</typeparam>
	/// <typeparam name="TResult">��� ������� ���������� ��������� ���������</typeparam>
	public class GetResultsCoreService<TMessageDomain, TGetStateResultProxy, TResultProxy, TResult> : ICoreService
		where TMessageDomain : MessageDomain
		where TResultProxy : IGetStateResult
	{
		private readonly IMessageDomainService<TMessageDomain> _messageDomainService;
		private readonly IGetResultProxyProvider<TGetStateResultProxy, TResultProxy> _getResultProxyProvider;
		private readonly IGetStateProxyConverter<TGetStateResultProxy, TMessageDomain> _getStateProxyConverter;
		private readonly IResultConverter<TResultProxy, TResult> _resultConverter;
		private readonly ISaveResultService<TResult, TMessageDomain> _saveResultService;
		private readonly IGetResultMessageHandler<TMessageDomain, TResult> _getResultMessageHandler;
		private readonly IGisLogger _logger;
		
		/// <summary>
		/// ���������� ����, ����� ������� ���������, ��� ������ �� ���������� �������
		/// </summary>
		private const int GET_RESULT_TIMEOUT_IN_DAYS = 3;

		public GetResultsCoreService(IMessageDomainService<TMessageDomain> messageDomainService,
			IGetResultProxyProvider<TGetStateResultProxy, TResultProxy> getResultProxyProvider,
			IGetStateProxyConverter<TGetStateResultProxy, TMessageDomain> getStateProxyConverter,
			IResultConverter<TResultProxy, TResult> resultConverter,
			ISaveResultService<TResult, TMessageDomain> saveResultService,
			IGetResultMessageHandler<TMessageDomain, TResult> getResultMessageHandler, IGisLogger logger)
		{
			_messageDomainService = messageDomainService;
			_getResultProxyProvider = getResultProxyProvider;
			_getStateProxyConverter = getStateProxyConverter;
			_resultConverter = resultConverter;
			_saveResultService = saveResultService;
			_getResultMessageHandler = getResultMessageHandler;
			_logger = logger;
		}

		public void Do(CoreInitData coreInitData)
		{
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			try
			{
				//�������� ��������� ��������� ��� �������� ���������� ���������
				var messages = _messageDomainService.GetMessageDomainsForGetResults(coreInitData);
				foreach (var messageDomain in messages)
				{
					try
					{
						//�� ��������� ��������� �������� getState ��� �������� ����������� ��������� ���������
						var getStateProxy = _getStateProxyConverter.ToGetStateResultProxy(messageDomain);
						TResultProxy resultProxy;
						//��������� ��������� ���������. 
						//���� ������������ false, ������ ��������� ��� �� ����������
						//���� true, ������ ����� �������� ��������� ���������
						if (_getResultProxyProvider.TryGetResult(getStateProxy, out resultProxy))
						{
							//���������� ����� ��������������� �� ������ �������� � ���� ������-�������� ���������� ���������
							var result = _resultConverter.ToResult(resultProxy);
							//��������� ��������� ��������� ���������
							_saveResultService.SaveResult(result, messageDomain);
							//����������� ������� ��������� ��������� � �������� ���������
							_getResultMessageHandler.Success(messageDomain, result);
						}
						else
						{
							if (messageDomain.SendedDate.HasValue 
								&& DateTime.Now.Subtract(messageDomain.SendedDate.Value).Days > GET_RESULT_TIMEOUT_IN_DAYS)
							{
								//� ������� �������� �� ����� �������� ��������� ��������� ���������, ��������
								_getResultMessageHandler.NoResultByTimeout(messageDomain);
							}
							else
							{
								//��������, ��� ��������� ��� �� ������������
								_getResultMessageHandler.NotReady(messageDomain);
							}
						}
					}
					catch (Exception exception)
					{
						//������������ ���������� �� ����� ������
						_getResultMessageHandler.Fail(messageDomain, exception);
					}
				}
				stopWatch.Stop();
				_logger.Info(this.GetType(), $"�� {messages.Count} �������� ���������� �� {coreInitData.UkId} �������� " +
							  $"{messages.Count(x => x.Status == MessageStatus.Done)} �������� �������, " +
							  $"{messages.Count(x => x.Status == MessageStatus.InProcess)} � ���������, " +
							  $"{messages.Count(x => x.Status == MessageStatus.ResponseTakingError)} ����� � �������, " +
							  $"{messages.Count(x => x.Status == MessageStatus.ResponseTakingErrorTryAgain)} ����� ���������� ��������, �� {stopWatch.Elapsed}");
			}
			catch (Exception ex)
			{
				_logger.Error(this.GetType(),$"��������� ���������� ��� ��������� {coreInitData}", ex);
			}
		}
	}
}