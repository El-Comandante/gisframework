using GisFramework.Domains;

namespace GisFramework.Interfaces.Converters
{
	/// <summary>
	/// �������������� ��������� ��������� � WCF ������ ������ ���������
	/// </summary>
	/// <typeparam name="TMessageDomain"></typeparam>
	/// <typeparam name="TMessageProxy">��� WCF ������ �������</typeparam>
	public interface IMessageProxyConverter<in TMessageDomain, out TMessageProxy>
		where TMessageDomain : MessageDomain
	{
		TMessageProxy ToMessageProxy(TMessageDomain messageDomain);
	}
}