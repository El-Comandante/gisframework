using System;
using GisFramework.Data;
using GisFramework.Domains;
using GisFramework.Exceptions;
using GisFramework.Interfaces.Handlers;
using GisFramework.Interfaces.Services;

namespace GisFramework.Handlers
{
	/// <summary>
	/// ������� ���������� ��������� ��������� ��� ��������� ���������� ��������� ���������
	/// </summary>
	/// <typeparam name="TMessageDomain">��� ��������� ���������</typeparam>
	/// <typeparam name="TResult">��� �������� ���������� ��������� ���������</typeparam>
	public abstract class GetResultMessageHandlerBase<TMessageDomain, TResult> : IGetResultMessageHandler<TMessageDomain, TResult>
		where TMessageDomain : MessageDomain
	{
		private readonly IGisLogger _logger;
		private readonly IMessageDomainService<TMessageDomain> _messageDomainService;

		protected GetResultMessageHandlerBase(IGisLogger logger, IMessageDomainService<TMessageDomain> messageDomainService)
		{
			_logger = logger;
			_messageDomainService = messageDomainService;
		}

		/// <summary>
		/// ��������� ��������� ��������� ���������� ��������� ���������
		/// </summary>
		/// <param name="messageDomain"></param>
		/// <param name="result"></param>
		public abstract void Success(TMessageDomain messageDomain, TResult result);

		/// <summary>
		/// ��������� ��� �� ���������� � ��� ���
		/// </summary>
		/// <param name="messageDomain">��� ��������� ���������</param>
		public void NotReady(TMessageDomain messageDomain)
		{
			messageDomain.Status = MessageStatus.InProcess;
			_messageDomainService.Update(messageDomain);
		}
		
		/// <summary>
		/// � ������� �������� �� ����� �������� ��������� ��������� ���������
		/// </summary>
		/// <param name="messageDomain"></param>
		public void NoResultByTimeout(TMessageDomain messageDomain)
		{
			messageDomain.Status = MessageStatus.NoResultByTimeout;
			_messageDomainService.Update(messageDomain);
		}

		/// <summary>
		/// ��� ��������� ���������� ��������� ��������� ����������
		/// </summary>
		/// <param name="messageDomain">��� ��������� ���������</param>
		/// <param name="exception">����������</param>
		public void Fail(TMessageDomain messageDomain, Exception exception)
		{
			//�� ��������� ��� ��������� ����� ����������� ��������� ��� ���
			var status = MessageStatus.ResponseTakingErrorTryAgain;
			//������ ������������ ������ �� �������� ��������� ��������
			if (exception is IntegrationServerErrorException)
			{
				status = MessageStatus.ResponseTakingError;
			}
			messageDomain.Status = status;
			messageDomain.ErrorText = exception.Message;
			_messageDomainService.Update(messageDomain);
			_logger.Error(exception);
		}
	}
}