namespace GisFramework.Interfaces.Providers
{
	/// <summary>
	/// ����� ���������� �������� �� ���������� �������� � ��� ���
	/// </summary>
	/// <typeparam name="TMessageProxy">��� ������������� WCF ������ �������</typeparam>
	/// <typeparam name="TAckProxy">��� Ack ��� ������� ��� ���</typeparam>
	public interface ISendMessageProxyProvider<in TMessageProxy, out TAckProxy> 
		where TAckProxy : IAckRequestAck
	{
		TAckProxy SendMessage(TMessageProxy messageProxy);
	}
}