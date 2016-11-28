using System;
using GisFramework.Data;
using GisFramework.Domains;
using GisFramework.Exceptions;
using GisFramework.Interfaces;
using GisFramework.Interfaces.Handlers;
using GisFramework.Interfaces.Services;

namespace GisFramework.Handlers
{
	/// <summary>
	/// ���������� ��������� ��������� ��� �������� ���������
	/// </summary>
	/// <typeparam name="TMessageDomain">��� ��������� ���������</typeparam>
	/// <typeparam name="TAckProxy">��� ������ ������� ������ �� �������� ���������</typeparam>
	public class SendMessageHandler<TMessageDomain, TAckProxy> : ISendMessageHandler<TMessageDomain, TAckProxy>
		where TMessageDomain : MessageDomain
		where TAckProxy : IAckRequestAck
	{
		private readonly IGisLogger _logger;
		private readonly IMessageDomainService<TMessageDomain> _messageDomainService;

		public SendMessageHandler(IGisLogger logger,
			IMessageDomainService<TMessageDomain> messageDomainService)
		{
			_logger = logger;
			_messageDomainService = messageDomainService;
		}

		/// <summary>
		/// �������� ��������� ������ �������.
		/// </summary>
		/// <param name="messageDomain">��� ��������� ���������</param>
		/// <param name="ackProxy"></param>
		public void SendSuccess(TMessageDomain messageDomain, TAckProxy ackProxy)
		{
			messageDomain.Status = MessageStatus.Sent;
			messageDomain.Sended = DateTime.Now;
			messageDomain.ResponseGuid = new Guid(ackProxy.MessageGUID);
			_messageDomainService.Update(messageDomain);
		}

		/// <summary>
		/// ��� �������� ��������� ��������� ����������
		/// </summary>
		/// <param name="messageDomain">��� ��������� ���������</param>
		/// <param name="exception">����������</param>
		public void SendFail(TMessageDomain messageDomain, Exception exception)
		{
			//�� ��������� ��� ��������� ����� ����������� ��������� ��� ���
			var status = MessageStatus.SendErrorTryAgain;
			//������ ������������ ������ �� �������� ��������� ��������
			if (exception is IntegrationServerErrorException)
			{
				status = MessageStatus.SendError;
			}
			messageDomain.Status = status;
			messageDomain.ErrorText = exception.Message;
			_messageDomainService.Update(messageDomain);
			_logger.Error(exception);
		}
	}
}