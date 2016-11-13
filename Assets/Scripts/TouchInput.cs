using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchInput : MonoBehaviour
{
    public enum TypeOfInput { MOVE, FIRE }

    public TypeOfInput typeOfInput;

    private RectTransform pointRect;

    private RectTransform mainRect;
    private Image pointImage;
    private PlayerManager playerController;

    private bool isTouch;
    private int touchFingerId;
    private float maxRadiusInput;

    private Vector2 startPosition;

    private const string pathPoint = "Point";

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerManager>();

        pointRect = transform.Find(pathPoint).GetComponent<RectTransform>();
        pointImage = pointRect.GetComponent<Image>();
        mainRect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        maxRadiusInput = Screen.width / 6f;
    }

    private void Update()
    {
#if UNITY_EDITOR
        EditorInput();
#elif UNITY_ANDROID
        MobileInput();
#endif
    }

    private void EditorInput()
    {
        //if (Input.GetMouseButton(0))
        //{
        //    if (!isTouch && IsTouchInsideRect(Input.mousePosition, mainRect))
        //    {
        //        isTouch = true;
        //        pointImage.color = Color.white;
        //        startPosition = Input.mousePosition;
        //    }

        //    if (isTouch)
        //    {           
        //        Vector2 inputDirection = (Vector2)Input.mousePosition - startPosition;

        //        float inputMagnitude = inputDirection.magnitude + .1f;

        //        float currentInputRadius = Mathf.Clamp(inputMagnitude, .1f, maxRadiusInput);
        //        float currentInputRadiusPercent = currentInputRadius / maxRadiusInput;

        //        Vector2 pointPosition = inputDirection * (currentInputRadius / inputMagnitude);
        //        pointRect.position = pointPosition + startPosition;

        //        Vector2 moveDirection2D = inputDirection.normalized * currentInputRadiusPercent;
        //        Vector3 moveDirection = new Vector3(moveDirection2D.x, 0f, moveDirection2D.y);

        //        switch (typeOfInput)
        //        {
        //            case TypeOfInput.MOVE:
        //                playerController.Move(moveDirection);
        //                break;
        //            case TypeOfInput.FIRE:
        //                playerController.Look(inputDirection.normalized);
        //                playerController.Fire();
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}
        //else
        //{
        //    pointImage.color = Color.clear;
        //    isTouch = false;
        //}


    }

    private void MobileInput()
    {
        if (!isTouch)
        {
            foreach (var item in Input.touches)
            {
                if (IsTouchInsideRect(item.position, mainRect) && item.phase == TouchPhase.Moved)
                {
                    touchFingerId = item.fingerId;
                    startPosition = item.position;
                    isTouch = true;
                    pointImage.color = Color.white;
                }
            }
        }

        if (isTouch)
        {
            Vector2 inputDirection = Input.GetTouch(touchFingerId).position - new Vector2(mainRect.position.x, mainRect.position.y);

            float inputMagnitude = inputDirection.magnitude + .1f;

            float currentInputRadius = Mathf.Clamp(inputMagnitude, .1f, maxRadiusInput);
            float currentInputRadiusPercent = currentInputRadius / maxRadiusInput;

            Vector2 pointPosition = inputDirection * (currentInputRadius / inputMagnitude);
            pointRect.position = pointPosition + startPosition;

            Vector2 moveDirection2D = inputDirection.normalized * currentInputRadiusPercent;
            Vector3 moveDirection = new Vector3(moveDirection2D.x, 0f, moveDirection2D.y);

            switch (typeOfInput)
            {
                case TypeOfInput.MOVE:
                    playerController.Move(moveDirection);
                    break;
                case TypeOfInput.FIRE:
                    playerController.Look(inputDirection.normalized);
                    playerController.Fire();
                    break;
                default:
                    break;
            }
        }

        if (Input.GetTouch(touchFingerId).phase == TouchPhase.Ended || Input.GetTouch(touchFingerId).phase == TouchPhase.Canceled)
        {
            isTouch = false;
            pointImage.color = Color.clear;
        }
    }

    private bool IsTouchInsideRect(Vector2 touchPosition, RectTransform rectTrans)
    {
        if (touchPosition.x >= (rectTrans.position.x - rectTrans.sizeDelta.x / 2) && touchPosition.x <= (rectTrans.position.x + rectTrans.sizeDelta.x / 2) 
            && touchPosition.y >= (rectTrans.position.y - rectTrans.sizeDelta.y / 2) && touchPosition.y <= (rectTrans.position.y + rectTrans.sizeDelta.y / 2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
