using System;

public interface IAction
{
    public void AddTagAction(TagAction action);

    public void ExecuteTagActions(Action callback);
}
