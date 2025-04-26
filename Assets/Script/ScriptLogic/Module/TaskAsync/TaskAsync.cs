using System.Threading.Tasks;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using UnityEngine;

public class TaskAsync
{
    public enum Type
    {
        Task,
        Sequence,
    }

    int id;
    string name;
    AsyncOperation asyncOperation;
    OperationSequence sequence;
    Type type;

    public bool IsDone {
        get {
            return (Progress == 1);
        }
    }


    public float Progress
    {
        get {
            if (asyncOperation != null){
                return asyncOperation.progress;
            }
            return sequence.Progress;
        }
    }


    // public AsyncOperation AsyncOperation => asyncOperation;
    public string Name => name;
    public int Id => id;
    public Type TaskType => type;
    public override int GetHashCode()
    {
        if (type == Type.Task){
            return asyncOperation.GetHashCode();
        }
        else {
            return sequence.GetHashCode();
        }
    }

    public TaskAsync(AsyncOperation asyncOperation, string name, int id){
        this.asyncOperation = asyncOperation;
        this.name = name;
        this.id = id;
        this.type = Type.Task;
    }

    public TaskAsync(OperationSequence sequence, string name, int id){
        this.sequence = sequence;
        this.name = name;
        this.id = id;
        this.type = Type.Sequence;
    }
}