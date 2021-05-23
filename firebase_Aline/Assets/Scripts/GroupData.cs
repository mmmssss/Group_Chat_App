using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GroupData{
  public string roomIDPass;
  public Dictionary<string,string> chats;
  public List<Dictionary<string, string>> members;
  public List<Dictionary<string,string>> messages;
}
