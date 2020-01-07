
using Mirror;

public class NetworkName : NetworkBehaviour
{
   
    public override bool OnSerialize(NetworkWriter writer, bool initialState)
    {
        writer.WriteString(name);
        return true;
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        name = reader.ReadString();
    }
    
}
