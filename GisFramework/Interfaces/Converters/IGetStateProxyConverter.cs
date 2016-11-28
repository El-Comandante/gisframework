using GisFramework.Domains;

namespace GisFramework.Interfaces.Converters
{
	/// <summary>
	/// �������������� �������� �������� � ������ �� ��������� ���������� ���������
	/// </summary>
	/// <typeparam name="TGetStateResultProxy"></typeparam>
	/// <typeparam name="TMessageDomain"></typeparam>
	public interface IGetStateProxyConverter<out TGetStateResultProxy, in TMessageDomain>
		where TMessageDomain : MessageDomain
	{
		TGetStateResultProxy ToGetStateResultProxy(TMessageDomain messageDomain);
	}
}