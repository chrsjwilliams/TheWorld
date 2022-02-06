using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DialogGraphParser : MonoBehaviour
{
    [SerializeField] DialogGraph dialogGraph;

    [SerializeField] DialogNode currentNode;

    [SerializeField] Stack<DialogNode> visitedNodes;

    [SerializeField] TextMeshProUGUI dialogText;

    [SerializeField] Image playerProfile;
    [SerializeField] Image npcProfile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
