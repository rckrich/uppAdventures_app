using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


public class ScrollRectReloaderObject : AppObject, IEndDragHandler, IBeginDragHandler, IDragHandler
{
    //References -> https://answers.unity.com/questions/1544176/scrollviewscrollrect-scrolling-refresh-event.html

    [Header("Labels")]
    private string topPullLabel = "BAJA MÁS PARA ACTUALIZAR";
    private string topReleaseLabel = "SUELTA PARA ACTUALIZAR";
    private string bottomPullLabel = "SUBE MÁS PARA CARGAR ACTIVIDADES ANTERIORES";
    private string bottomReleaseLabel = "SUELTA PARA CARGAR ACTIVIDADES ANTERIORES";

    [Header("Limiter References")]
    public Transform loadMoreObjectPosition;
    public Transform loadMorePivotBottom;

    [Header("Visuals")]
    public bool visualsActive = true;
    public GameObject reloadEventObject;
    public Text reloadEventsText;

    [Header("Settings")]
    public bool topLoader = true;
    public bool loading = false;
    public bool enableScrollLoadMore = true;

    [Header("Pull coefficient")]
    [Range(0f, 1f)]
    public float loadMoreObjectsLimiter = 0.15f;

    private float loadMorePivotValue;
    private bool _isCanLoadUp;
    private Action actionToReload;

    public void OnDrag(PointerEventData eventData)
    {

        if (loading || !enableScrollLoadMore)
            return;

        if (topLoader)
        {
            if ((loadMorePivotBottom.position.y < loadMoreObjectPosition.position.y))
            {
                if (visualsActive) {
                    reloadEventObject.SetActive(true);
                    reloadEventsText.text = topPullLabel;
                }

                if (loadMorePivotBottom.position.y < (loadMorePivotValue - loadMoreObjectsLimiter))
                {
                    if(visualsActive)
                        reloadEventsText.text = topReleaseLabel;
                    _isCanLoadUp = true;
                }
                else
                {
                    _isCanLoadUp = false;
                }

            }
            else
            {
                if (visualsActive) {
                    reloadEventsText.text = topPullLabel;
                    reloadEventObject.SetActive(false);
                }

                _isCanLoadUp = false;
            }
        }
        else {
            if ((loadMorePivotBottom.position.y > loadMoreObjectPosition.position.y))
            {
                if (visualsActive) {
                    reloadEventObject.SetActive(true);
                    reloadEventsText.text = bottomPullLabel;
                }

                if (loadMorePivotBottom.position.y > (loadMorePivotValue + loadMoreObjectsLimiter))
                {
                    if (visualsActive)
                        reloadEventsText.text = bottomReleaseLabel;
                    _isCanLoadUp = true;
                }
                else
                {
                    _isCanLoadUp = false;
                }

            }
            else {
                if (visualsActive) {
                    reloadEventsText.text = bottomPullLabel;
                    reloadEventObject.SetActive(false);
                }
                _isCanLoadUp = false;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isCanLoadUp && actionToReload != null)
        {
            actionToReload();
        }
        _isCanLoadUp = false;
        if(visualsActive)
            reloadEventObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        loadMorePivotValue = loadMorePivotBottom.position.y;
    }

    public void SetReloadAction(System.Action actionToReload)
    {
        this.actionToReload = actionToReload;
    }

}
