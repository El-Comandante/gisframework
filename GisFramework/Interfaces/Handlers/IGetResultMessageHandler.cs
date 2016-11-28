using System;
using GisFramework.Domains;

namespace GisFramework.Interfaces.Handlers
{
	/// <summary>
	/// ���������� �������� �������� ��� ��������� ���������� ���������
	/// </summary>
	/// <typeparam name="TMessageDomain"></typeparam>
	/// <typeparam name="TResultProxy"></typeparam>
	public interface IGetResultMessageHandler<in TMessageDomain, in TResultProxy> 
		where TMessageDomain : MessageDomain
	{
		void Success(TMessageDomain messageDomain, TResultProxy result);
		void NotReady(TMessageDomain messageDomain);
		void NoResultByTimeout(TMessageDomain messageDomain);
		void Fail(TMessageDomain messageDomain, Exception exception);
	}
}