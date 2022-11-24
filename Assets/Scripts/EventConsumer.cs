using UnityEngine;

public class EventConsumer: MonoBehaviour
{
    public void Stop(string Font)
    {
        Debug.Log("Step");
    }
    public void PunchSound1(AnimationEvent _AnimationEvent)
    {
        string l_StringParmeter = _AnimationEvent.stringParameter;
        float l_FloatParameter = _AnimationEvent.floatParameter;
        int l_IntParameter = _AnimationEvent.intParameter;
        Object l_Object = _AnimationEvent.objectReferenceParameter;
        Debug.Log("event _punchsound " + l_StringParmeter);
        //ESTO ENTRA EN EXAMEN, PASAR DE CORDENADADS DE MUNDO A LOCAL Y EL BLENDTREE
    }

}
