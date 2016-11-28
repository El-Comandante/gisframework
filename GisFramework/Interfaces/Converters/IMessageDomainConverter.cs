using System;
using System.Collections.Generic;
using GisFramework.Data;
using GisFramework.Domains;

namespace GisFramework.Interfaces.Converters
{
	/// <summary>
	/// ��������� ��������, ����������� �� �������������� ������� � �������� ���������
	/// </summary>
	/// <typeparam name="TMessageDomain"></typeparam>
	/// <typeparam name="TSourceDomain"></typeparam>
	public interface IMessageDomainConverter<TMessageDomain, TSourceDomain>
		where TMessageDomain : MessageDomain
	{
		/// <summary>
		/// ��� �������� ������ � ��� ��� ��� ������ N TSourceDomain ��������� ���� �������� ���������
		/// ��� ��������� ������ �� ��� ��� ����� ��� ������� TSourceDomain ��������� TMessageDomain
		/// </summary>
		/// <param name="sourceDomains"></param>
		/// <param name="coreInitData"></param>
		/// <param name="senderGuid"></param>
		/// <returns></returns>
		List<TMessageDomain> ToMessageDomain(List<TSourceDomain> sourceDomains, CoreInitData coreInitData, Guid senderGuid);
	}
}