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
		/// <summary>
		/// ��������� ���������� ��� ������� �����������
		/// </summary>
		/// <param name="messageDomain"></param>
		/// <param name="result"></param>
		void Success(TMessageDomain messageDomain, TResultProxy result);

		/// <summary>
		/// ��������� ���������� ��� ���������, ��������� � ��� ��� ��� �� �����
		/// </summary>
		/// <param name="messageDomain"></param>
		void NotReady(TMessageDomain messageDomain);

		/// <summary>
		/// �� ��������� ����� �� ����� �������� ���������, �������� ��� ������� �� ��������
		/// </summary>
		/// <param name="messageDomain"></param>
		void NoResultByTimeout(TMessageDomain messageDomain);

		/// <summary>
		/// ��� ��������� ��������� ��������� ������
		/// </summary>
		/// <param name="messageDomain"></param>
		/// <param name="exception"></param>
		void Fail(TMessageDomain messageDomain, Exception exception);
	}
}