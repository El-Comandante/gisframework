namespace GisFramework.Interfaces.Providers
{
	/// <summary>
	/// ���������� �������� ���������
	/// </summary>
	/// <typeparam name="TMessageProxy"></typeparam>
	/// <typeparam name="TAckProxy"></typeparam>
	public interface ISendMessageProxyProvider<in TMessageProxy, out TAckProxy> 
		where TAckProxy : IAckRequestAck
	{
		TAckProxy SendMessage(TMessageProxy messageProxy);
	}
}