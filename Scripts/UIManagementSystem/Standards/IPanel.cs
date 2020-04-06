using UnityEngine;

public interface IPanel
{
    string Name { get; }
    GameObject panelObject { get;  }
    void Activate();
    void Deactivate();
}