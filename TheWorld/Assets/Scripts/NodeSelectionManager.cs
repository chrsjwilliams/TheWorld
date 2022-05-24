using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodeSelectionManager : MonoBehaviour
{
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;
    [SerializeField] RectTransform nodePageParent;
    [SerializeField] HorizontalLayoutGroup layoutGroup;
    [SerializeField] GridLayoutGroup firstPage;
    [SerializeField] List<GridLayoutGroup> nodeButtonPages;
    [SerializeField] IntVariable NodeSelectionXPos;
    [SerializeField] RectTransformTweener layoutGroupTweener;
    [SerializeField] NodeButton nodeButtonPrefab;
    [SerializeField] GridLayoutGroup nodeButtonPagePrefab;
    [SerializeField] int nodesPerPage;
    [SerializeField] TextMeshProUGUI windowTitle;

    public MonoTweener fadeInTween;
    public MonoTweener fadeOutTween;

    [SerializeField] List<NodeButton> nodeButtons = new List<NodeButton>();
    int optionWidth;
    int nodePageIndex = 0;
    bool isLerping = false;

    // Start is called before the first frame update
    void Start()
    {
        optionWidth = 1000;
        nodeButtonPages.Add(firstPage);

        leftButton.interactable = false;

        RefreshLayoutGroup();
        // Load all stamp packs

    }

    public void RefreshLayoutGroup()
    {
        // This is used to update the layout group
        Canvas.ForceUpdateCanvases();
        layoutGroup.enabled = false;
        layoutGroup.enabled = true;
    }

    public void RemoveNodeButtons()
    {
        foreach(GridLayoutGroup page in nodeButtonPages)
        {
            if(page != firstPage)
            {
                Destroy(page.gameObject);
            }
        }

        List<NodeButton> buttonsToDelete = new List<NodeButton>();
        buttonsToDelete.AddRange(nodeButtons);
        foreach(NodeButton button in buttonsToDelete)
        {
            Destroy(button.gameObject);
        }

        nodeButtons = new List<NodeButton>();
    }

    public void PopulateOption(List<DialogNode> nodes,string title ,bool interactable = true)
    {
        //RemoveNodeButtons();
        windowTitle.text = title;

        int nodesOnPage = 0;
        GridLayoutGroup currentPage = firstPage;
        foreach (DialogNode node in nodes)
        {
            //  If the first page is full and we are on the 8th stamp,
            //  create a new page
            if (nodesOnPage % nodesPerPage == 0 && firstPage.transform.childCount >= nodesPerPage)
            {
                currentPage = Instantiate(nodeButtonPagePrefab, nodePageParent);

                nodeButtonPages.Add(currentPage);
                nodesOnPage = 0;
            }

            // create instance of MBSharingUIOption and populate it
            // with the metadata and increase the number of stamps on the page
            var nodeButton = Instantiate(nodeButtonPrefab, currentPage.transform);
            nodeButton.Init(node.name, canVisit: interactable);
            nodeButtons.Add(nodeButton);
            nodesOnPage++;
        }

        // If there is only one page of stamps, disable the left and right buttons
        if (nodeButtonPages.Count == 1)
        {
            leftButton.interactable = false;
            rightButton.interactable = false;
        }
    }


    public void FinishedLerping()
    {
        isLerping = false;
    }

    public void DirectionButtonPressed(DirectionButton button)
    {
        if (isLerping) return;
        if (nodeButtonPages.Count < 1) return;
        if (nodeButtonPages.Count == 1)
        {
            leftButton.interactable = false;
            rightButton.interactable = false;
            return;
        }

        int dirValue = 1;
        if (button.direction == DirectionButton.Direction.LEFT)
        {
            nodePageIndex -= 1;
            dirValue = -1;
            if (nodePageIndex <= 0)
            {
                nodePageIndex = 0;
            }
        }
        else if (button.direction == DirectionButton.Direction.RIGHT)
        {
            nodePageIndex += 1;
            dirValue = 1;
            if (nodePageIndex <= 0)
            {
                nodePageIndex = 0;
            }
        }

        if (nodePageIndex == 0)
        {
            leftButton.interactable = false;
        }
        else
        {
            leftButton.interactable = true;
        }

        if (nodePageIndex == nodeButtonPages.Count - 1)
        {
            rightButton.interactable = false;
        }
        else
        {
            rightButton.interactable = true;
        }


        isLerping = true;
        NodeSelectionXPos.value = (int)(nodePageParent.anchoredPosition.x - ((optionWidth + layoutGroup.spacing) * dirValue));
        layoutGroupTweener.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
