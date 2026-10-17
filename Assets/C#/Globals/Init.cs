using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using TMPro;


public class Init : MonoBehaviour
{
    //CanvasMain
    [SerializeField] Canvas cnvMain;

    // Start is called before the first frame update
    void Start()
    {
        //Canvas
        Globals.CANVAS_MENU = cnvMain.transform.Find("CanvasMenu").GetComponent<Canvas>();
        Globals.CANVAS_START = cnvMain.transform.Find("CanvasInizia").GetComponent<Canvas>();
        Globals.CANVAS_WEB = cnvMain.transform.Find("CanvasWeb").GetComponent<Canvas>();
        Globals.CANVAS_POPUP = cnvMain.transform.Find("CanvasPopup").GetComponent<Canvas>();
        //Scenes Menu
        Globals.SCENE_MENU = Globals.CANVAS_MENU.transform.Find("Menu").gameObject;
        Globals.SCENE_SCHEDE = Globals.CANVAS_MENU.transform.Find("Tabs").gameObject;
        Globals.SCENE_ESERCIZI = Globals.CANVAS_MENU.transform.Find("Exercises").gameObject;
        //Pages
        Globals.PAGE_MENU = Globals.SCENE_MENU.transform.Find("PageArea").gameObject;

        Globals.PAGE_MENU_STATS = Globals.PAGE_MENU.transform.Find("P1 Stats").gameObject;
        Globals.PAGE_MENU_TRAINING = Globals.PAGE_MENU.transform.Find("P2 Training").gameObject;
        Globals.PAGE_MENU_EDIT = Globals.PAGE_MENU.transform.Find("P3 Edit").gameObject;

        Globals.PAGE_TAB_VIEW = Globals.SCENE_SCHEDE.transform.Find("Tabs View").gameObject;

        Globals.PAGE_EXER_VIEW = Globals.SCENE_ESERCIZI.transform.Find("Exercises View").gameObject;    
        Globals.PAGE_EXER_VIEW_DET = Globals.SCENE_ESERCIZI.transform.Find("Details View").gameObject;
        Globals.PAGE_EXER_ADD = Globals.SCENE_ESERCIZI.transform.Find("Exercises Add").gameObject;
        Globals.PAGE_EXER_ADD_DET = Globals.SCENE_ESERCIZI.transform.Find("Details Ex Add").gameObject;


        Globals.PAGE_SCHEDE = Globals.CANVAS_START.transform.Find("Schede").gameObject;
        Globals.PAGE_ESERCIZI = Globals.CANVAS_START.transform.Find("Esercizi").gameObject;
        Globals.PAGE_ALLENAMENTO = Globals.CANVAS_START.transform.Find("Allenamento").gameObject;
        Globals.PAGE_FINITO = Globals.CANVAS_START.transform.Find("Finito").gameObject;
        
        Canvas cnvRecupero = Globals.PAGE_ALLENAMENTO.transform.Find("Recupero").GetComponent<Canvas>();
        Globals.CNV_ESE_TIMER = cnvRecupero.transform.Find("CnvTimer").GetComponent<Canvas>();
        Globals.CNV_BTN_TIMER = cnvRecupero.transform.Find("CnvButton").GetComponent<Canvas>();
        Canvas cnvEsercizio = Globals.PAGE_ALLENAMENTO.transform.Find("Esercizio").GetComponent<Canvas>();
        Globals.CNV_ESE_REPS = cnvEsercizio.transform.Find("CnvEsercizio").GetComponent<Canvas>();
        Globals.CNV_WEIGHT = cnvEsercizio.transform.Find("CnvWeight").GetComponent<Canvas>();
        Globals.CNV_BTN_REPS = cnvEsercizio.transform.Find("CnvButton").GetComponent<Canvas>();
        //Popup
        Globals.PP_MUSCOLI = Globals.CANVAS_POPUP.transform.Find("Muscles").gameObject;
        Globals.PP_BUTTON = Globals.CANVAS_POPUP.transform.Find("Button Area").gameObject;
        //Tabs
        GameObject goTabArea = Globals.SCENE_MENU.transform.Find("Tab Area").gameObject;
        Globals.TAB_STATS = goTabArea.transform.Find("Tab Stats").gameObject;
        Globals.TAB_TRAINING = goTabArea.transform.Find("Tab Training").gameObject;
        Globals.TAB_EDIT = goTabArea.transform.Find("Tab Edit").gameObject;

        Globals.TAB_ANSWER = Globals.PP_BUTTON.transform.Find("Tab Answer").gameObject;
        //Button
        Globals.BTN_CONFIRM = Globals.TAB_ANSWER.transform.Find("btnConfirm").GetComponent<Button>(); 
        Globals.BTN_CANCEL = Globals.TAB_ANSWER.transform.Find("btnCancel").GetComponent<Button>();

        Globals.BTN_ADD_SCHEDA = Globals.PAGE_TAB_VIEW.transform.Find("Scroll View/Viewport/Content/Aggiungi").GetComponent<Button>();
        Globals.BTN_ADD_ESERCIZIO = Globals.PAGE_EXER_VIEW.transform.Find("Scroll View/Viewport/Content/Aggiungi").GetComponent<Button>();
        Globals.BTN_ADD_ESERCIZIO_DET = Globals.PAGE_EXER_VIEW_DET.transform.Find("Scroll View/Viewport/Content/Aggiungi").GetComponent<Button>();

        Globals.BTN_ADD_SCHEDA_ALNMT = Globals.PAGE_SCHEDE.transform.Find("Scroll View/Viewport/Content/Aggiungi").GetComponent<Button>();
        GameObject goDetailsView = Globals.PAGE_ESERCIZI.transform.Find("DetailsView").gameObject;
        GameObject goExploseView = Globals.PAGE_ESERCIZI.transform.Find("ExploseView").gameObject;
        Globals.BTN_ADD_ESERCIZIO_DET_ALNMT = goDetailsView.transform.Find("Scroll View/Viewport/ContentDet/Aggiungi").GetComponent<Button>();
        Globals.BTN_ADD_ESERCIZIO_EXPL_ALNMT = goExploseView.transform.Find("Scroll View/Viewport/ContentExplose/Esercizio").GetComponent<Button>();
        Globals.BTN_ADD_ESERCIZIO_EXPL_REC_ALNMT = goExploseView.transform.Find("Scroll View/Viewport/ContentExplose/Recupero").GetComponent<Button>();

        Globals.BTN_HOME = cnvMain.transform.Find("btnHome").GetComponent<Button>();
        Globals.BTN_BACK = cnvMain.transform.Find("btnBack").GetComponent<Button>();

        Globals.BTN_SPALLE = Globals.PP_MUSCOLI.transform.Find("btnShoulders").GetComponent<Button>();
        Globals.BTN_PETTO = Globals.PP_MUSCOLI.transform.Find("btnChest").GetComponent<Button>();
        Globals.BTN_SCHIENA = Globals.PP_MUSCOLI.transform.Find("btnBack").GetComponent<Button>();
        Globals.BTN_BRACCIA = Globals.PP_MUSCOLI.transform.Find("btnArms").GetComponent<Button>();
        Globals.BTN_ABS = Globals.PP_MUSCOLI.transform.Find("btnABS").GetComponent<Button>();
        Globals.BTN_GAMBE = Globals.PP_MUSCOLI.transform.Find("btnLegs").GetComponent<Button>();

        Canvas cnvBarraEsercizi = Globals.PAGE_ALLENAMENTO.transform.Find("BarraEsercizi").GetComponent<Canvas>();
        Globals.BTN_PAST = cnvBarraEsercizi.transform.Find("svPast/Viewport/Content/Button").GetComponent<Button>();
        Globals.BTN_NEXT = cnvBarraEsercizi.transform.Find("btnNext").GetComponent<Button>();
        Globals.BTN_THEN = cnvBarraEsercizi.transform.Find("svThen/Viewport/Content/Button").GetComponent<Button>();
        Globals.BTN_FINITO_ESE = Globals.PAGE_FINITO.transform.Find("cnvRiepilogo/ScrollView/Viewport/Content/Esercizio").GetComponent<Button>();
        Globals.BTN_FINITO_TIM = Globals.PAGE_FINITO.transform.Find("cnvRiepilogo/ScrollView/Viewport/Content/Recupero").GetComponent<Button>();
        
        //Popup
        Globals.PP_ESERCIZIO = Globals.PAGE_ALLENAMENTO.transform.Find("popupEsercizio").GetComponent<Canvas>();
        Globals.PP_TIME = Globals.PAGE_FINITO.transform.Find("cnvRiepilogo/ppTime").GetComponent<Canvas>();


        Globals.COUNT_TIMER = 0;
        Globals.COUNT_FINITO = 0;
        Globals.SQL = 0;

        InitializeButton();

        AssignClasses();

        AppManager.Instance.navManager.NavigateTo(AppManager.Instance.objects.BtnHome);
    }

    private void InitializeButton()
    {
        cnvMain.transform.Find("btnHome").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.objects.BtnHome());
        cnvMain.transform.Find("btnBack").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.navManager.NavigateBack());

        //Menu
        Globals.SCENE_MENU.transform.Find("Tab Area/Tab Stats/btnStats").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.objects.ChangeToTab(0));
        Globals.SCENE_MENU.transform.Find("Tab Area/Tab Training/btnTraining").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.objects.ChangeToTab(1));
        Globals.SCENE_MENU.transform.Find("Tab Area/Tab Edit/btnEdit").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.objects.ChangeToTab(2));
        Globals.SCENE_MENU.transform.Find("PageArea/P2 Training/btnStart").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.navManager.NavigateTo(() => AppManager.Instance.objects.PagesTraining("Carica")));
        Globals.SCENE_MENU.transform.Find("PageArea/P3 Edit/Choose/btnExercises").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.navManager.NavigateTo(() => AppManager.Instance.objects.ExercisesSwitchState("View&C")));
        Globals.SCENE_MENU.transform.Find("PageArea/P3 Edit/Choose/btnTabs").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.navManager.NavigateTo(() => AppManager.Instance.objects.TabsSwitchState("View&C")));
        Globals.SCENE_MENU.transform.Find("PageArea/P3 Edit/btnWeb").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.navManager.NavigateTo(AppManager.Instance.web.Apri));

        //Button Area
        Globals.PP_BUTTON.transform.Find("Tab Answer/btnConfirm").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.objects.ChangeToTab(0));
        Globals.PP_BUTTON.transform.Find("Tab Answer/btnCancel").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.objects.ChangeToTab(1));

        //Esercizio
        Globals.SCENE_ESERCIZI.transform.Find("Exercises Add/PanelImage/btnImage").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.uploadImage.PickImage(Globals.SCENE_ESERCIZI.transform.Find("Exercises Add/PanelImage/Image").GetComponent<Image>()));

        //Allenamento
        Globals.CANVAS_START.transform.Find("btnEsci").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.navManager.NavigateTo(AppManager.Instance.objects.btnEsciAllenamento));
        Globals.PAGE_ALLENAMENTO.transform.Find("BarraSuperiore/btnFine").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.navManager.NavigateTo(AppManager.Instance.allenamento.Finito));
        //Allenamento Recupero
        Globals.PAGE_ALLENAMENTO.transform.Find("Recupero/CnvButton/btnBack").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.allenamento.Back());
        Globals.PAGE_ALLENAMENTO.transform.Find("Recupero/CnvButton/btnStop").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.allenamentoBottoni.btnPause());
        Globals.PAGE_ALLENAMENTO.transform.Find("Recupero/CnvButton/btnSkip").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.allenamento.Skip());
        Globals.PAGE_ALLENAMENTO.transform.Find("Recupero/CnvButton/-5").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.allenamentoBottoni.btnDecreaseRest(5));
        Globals.PAGE_ALLENAMENTO.transform.Find("Recupero/CnvButton/+5").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.allenamentoBottoni.btnIncreaseRest(5));
        Globals.PAGE_ALLENAMENTO.transform.Find("Recupero/CnvButton/+10").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.allenamentoBottoni.btnIncreaseRest(10));
        Globals.PAGE_ALLENAMENTO.transform.Find("Recupero/CnvButton/-10").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.allenamentoBottoni.btnDecreaseRest(10));

        //Allenamento Esercizio
        Globals.PAGE_ALLENAMENTO.transform.Find("Esercizio/CnvButton/CnvTimer/btnBack").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.allenamento.Back());
        Globals.PAGE_ALLENAMENTO.transform.Find("Esercizio/CnvButton/CnvTimer/btnStop").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.allenamentoBottoni.btnPause());
        Globals.PAGE_ALLENAMENTO.transform.Find("Esercizio/CnvButton/CnvTimer/btnSkip").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.allenamento.Skip());
        Globals.PAGE_ALLENAMENTO.transform.Find("Esercizio/CnvButton/CnvReps/btnBack").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.allenamento.Back());
        Globals.PAGE_ALLENAMENTO.transform.Find("Esercizio/CnvButton/CnvReps/btnEnd").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.allenamentoEsercizio.Finito(true));
        Globals.PAGE_ALLENAMENTO.transform.Find("Esercizio/CnvButton/CnvReps/btnSkip").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.allenamentoEsercizio.Finito(false));
        Globals.PAGE_ALLENAMENTO.transform.Find("Esercizio/CnvButton/CnvReps/btnSkip").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.allenamentoEsercizio.Finito(false));
        Globals.PAGE_ALLENAMENTO.transform.Find("Esercizio/CnvWeight/btnWeightDown").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.allenamento.I_D_Weight("D"));
        Globals.PAGE_ALLENAMENTO.transform.Find("Esercizio/CnvWeight/btnWeightUp").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.allenamento.I_D_Weight("I"));


        Globals.PAGE_ESERCIZI.transform.Find("btnNomeScheda").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.eserciziAllenamento.SwitchView());
        //Finito
        Globals.PAGE_FINITO.transform.Find("cnvRiepilogo/ppTime/cnvPanel/btnConfirm").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.finito.PopupTimeClose("Confirm"));
        Globals.PAGE_FINITO.transform.Find("cnvRiepilogo/ppTime/cnvPanel/btnCancel").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.finito.PopupTimeClose("Close"));

        //popupEsercizio
        Globals.PAGE_ALLENAMENTO.transform.Find("popupEsercizio/btnClose").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.allenamento.ClosePopupEsercizio());

        //Web
        //Globals.CANVAS_WEB.transform.Find("cnvEsercizi/cnvSearchMusc/btnMuscles").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.filterSearchMusc.MusclesShow());

    }

    private void AssignClasses()
    {
        //FilterSearchMusc
        //-Menu
        FilterSearchMusc fsmMenu = Globals.PAGE_EXER_VIEW.transform.Find("cnvSearchMusc").GetComponent<Canvas>().gameObject.AddComponent<FilterSearchMusc>();
        fsmMenu.btnFiltered = Globals.BTN_ADD_ESERCIZIO;
        fsmMenu.Init();
        //-Web
        FilterSearchMusc fsmWeb = Globals.CANVAS_WEB.transform.Find("cnvEsercizi/cnvSearchMusc").GetComponent<Canvas>().gameObject.AddComponent<FilterSearchMusc>();
        fsmWeb.btnFiltered = Globals.CANVAS_WEB.transform.Find("cnvEsercizi/Scroll View/Viewport/Content/btnEsercizio").GetComponent<Button>();
        fsmWeb.Init();
        //FilterSearch
        //-Menu
        FilterSearch fsMenu = Globals.SCENE_SCHEDE.transform.Find("Tabs View/cnvSearch").GetComponent<Canvas>().gameObject.AddComponent<FilterSearch>();
        fsMenu.btnFiltered = Globals.SCENE_SCHEDE.transform.Find("Tabs View/Scroll View/Viewport/Content/Aggiungi").GetComponent<Button>();
        fsMenu.Init();
        //-Allenamento
        FilterSearch fsAll = Globals.CANVAS_START.transform.Find("Schede/cnvSearch").GetComponent<Canvas>().gameObject.AddComponent<FilterSearch>();
        fsAll.btnFiltered = Globals.CANVAS_START.transform.Find("Schede/Scroll View/Viewport/Content/Aggiungi").GetComponent<Button>();
        fsAll.Init();
        //-Web
        FilterSearch fsWeb = Globals.CANVAS_WEB.transform.Find("cnvSchede/cnvSearch").GetComponent<Canvas>().gameObject.AddComponent<FilterSearch>();
        fsWeb.btnFiltered = Globals.CANVAS_WEB.transform.Find("cnvSchede/Scroll View/Viewport/Content/btnScheda").GetComponent<Button>();
        fsWeb.Init();

        //Details Ex Add
        //-IncrOrDecr
        IntIncOrDecr iodSeries = Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasSeries").GetComponent<Canvas>().gameObject.AddComponent<IntIncOrDecr>();
        iodSeries.Max = 30;
        iodSeries.Min = 1;

        IntIncOrDecr iodRepsN = Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRepsN").GetComponent<Canvas>().gameObject.AddComponent<IntIncOrDecr>();
        iodRepsN.Max = 100;
        iodRepsN.Min = 1;
        //-Press IncrOrDecr Time
        ButtonPressSeconds bpsuReps = Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRepsT/btnUp_ss").GetComponent<Button>().gameObject.AddComponent<ButtonPressSeconds>();
        bpsuReps.IncreseOrDecrease = "Increase";
        Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRepsT/btnUp_ss").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.dettagliEsercizi.ChangeTime("RI_S_I"));

        ButtonPressSeconds bpsdReps = Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRepsT/btnDown_ss").GetComponent<Button>().gameObject.AddComponent<ButtonPressSeconds>();
        bpsdReps.IncreseOrDecrease = "Decrease";
        Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRepsT/btnDown_ss").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.dettagliEsercizi.ChangeTime("RI_S_D"));

        ButtonPressSeconds bpsuWait = Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRest/CanvasWait/btnUp_ss").GetComponent<Button>().gameObject.AddComponent<ButtonPressSeconds>();
        bpsuWait.IncreseOrDecrease = "Increase";
        Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRest/CanvasWait/btnUp_ss").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.dettagliEsercizi.ChangeTime("AT_S_I"));

        ButtonPressSeconds bpsdWait = Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRest/CanvasWait/btnDown_ss").GetComponent<Button>().gameObject.AddComponent<ButtonPressSeconds>();
        bpsdWait.IncreseOrDecrease = "Decrease";
        Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRest/CanvasWait/btnDown_ss").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.dettagliEsercizi.ChangeTime("AT_S_D"));

        ButtonPressSeconds bpsuRest = Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRest/CanvasRest/btnUp_ss").GetComponent<Button>().gameObject.AddComponent<ButtonPressSeconds>();
        bpsuRest.IncreseOrDecrease = "Increase";
        Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRest/CanvasRest/btnUp_ss").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.dettagliEsercizi.ChangeTime("RE_S_I"));

        ButtonPressSeconds bpsdRest = Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRest/CanvasRest/btnDown_ss").GetComponent<Button>().gameObject.AddComponent<ButtonPressSeconds>();
        bpsdRest.IncreseOrDecrease = "Decrease";
        Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRest/CanvasRest/btnDown_ss").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.dettagliEsercizi.ChangeTime("RE_S_D"));

        ButtonPressMinutes bpmuReps = Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRepsT/btnUp_mm").GetComponent<Button>().gameObject.AddComponent<ButtonPressMinutes>();
        bpmuReps.IncreseOrDecrease = "Increase";
        Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRepsT/btnUp_mm").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.dettagliEsercizi.ChangeTime("RI_M_I"));

        ButtonPressMinutes bpmdReps = Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRepsT/btnDown_mm").GetComponent<Button>().gameObject.AddComponent<ButtonPressMinutes>();
        bpmdReps.IncreseOrDecrease = "Decrease";
        Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRepsT/btnDown_mm").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.dettagliEsercizi.ChangeTime("RI_M_D"));

        ButtonPressMinutes bpmuWait = Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRest/CanvasWait/btnUp_mm").GetComponent<Button>().gameObject.AddComponent<ButtonPressMinutes>();
        bpmuWait.IncreseOrDecrease = "Increase";
        Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRest/CanvasWait/btnUp_mm").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.dettagliEsercizi.ChangeTime("AT_M_I"));

        ButtonPressMinutes bpmdWait = Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRest/CanvasWait/btnDown_mm").GetComponent<Button>().gameObject.AddComponent<ButtonPressMinutes>();
        bpmdWait.IncreseOrDecrease = "Decrease";
        Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRest/CanvasWait/btnDown_mm").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.dettagliEsercizi.ChangeTime("AT_M_D"));

        ButtonPressMinutes bpmuRest = Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRest/CanvasRest/btnUp_mm").GetComponent<Button>().gameObject.AddComponent<ButtonPressMinutes>();
        bpmuRest.IncreseOrDecrease = "Increase";
        Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRest/CanvasRest/btnUp_mm").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.dettagliEsercizi.ChangeTime("RE_M_I"));

        ButtonPressMinutes bpmdRest = Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRest/CanvasRest/btnDown_mm").GetComponent<Button>().gameObject.AddComponent<ButtonPressMinutes>();
        bpmdRest.IncreseOrDecrease = "Decrease";
        Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRest/CanvasRest/btnDown_mm").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.dettagliEsercizi.ChangeTime("RE_M_D"));
        //-RadioButton
        Globals.PAGE_EXER_ADD_DET.transform.Find("RadioButton/OptionReps").GetComponent<Toggle>().onValueChanged.AddListener((bool value) => AppManager.Instance.dettagliEsercizi.RadioButton_Change(value ? "N" : "T"));
        //-InputField dettagli
        Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRepsN/InputFieldReps").GetComponent<TMP_InputField>().onValueChanged.AddListener((string text) => AppManager.Instance.dettagliEsercizi.CheckValueIF(text, 100));
        Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasSeries/InputFieldSeries").GetComponent<TMP_InputField>().onValueChanged.AddListener((string text) => AppManager.Instance.dettagliEsercizi.CheckValueIF(text, 20));

        //Grid layout
        Globals.CANVAS_WEB.transform.Find("cnvPacchetti/svPacchetti/Viewport/Content").gameObject.AddComponent<LayoutElement>();
        GridLayoutGroup glgContentPacchetti = Globals.CANVAS_WEB.transform.Find("cnvPacchetti/svPacchetti/Viewport/Content").gameObject.AddComponent<GridLayoutGroup>();
        glgContentPacchetti.cellSize = new Vector2(200, 200);
        glgContentPacchetti.startCorner = GridLayoutGroup.Corner.UpperLeft;
        glgContentPacchetti.startAxis = GridLayoutGroup.Axis.Horizontal;
        Globals.CANVAS_WEB.transform.Find("cnvPacchetti/svPacchetti/Viewport/Content/cnvCell").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
        HorizontalLayoutGroup glgCellPacchetti = Globals.CANVAS_WEB.transform.Find("cnvPacchetti/svPacchetti/Viewport/Content/cnvCell").gameObject.AddComponent<HorizontalLayoutGroup>();
        glgCellPacchetti.childAlignment = TextAnchor.MiddleCenter;
        glgCellPacchetti.childControlHeight = false;
        glgCellPacchetti.childControlWidth = false;
        Globals.CANVAS_WEB.transform.Find("cnvPacchetti/svPacchetti/Viewport/Content/cnvCell/btnPacchetto").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 180);
        GameObject cnvLastRow = Globals.CANVAS_WEB.transform.Find("cnvPacchetti/svPacchetti/Viewport/Content/cnvLastRow").gameObject;
        cnvLastRow.AddComponent<LayoutElement>().ignoreLayout = true;
        cnvLastRow.AddComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;
        cnvLastRow.GetComponent<HorizontalLayoutGroup>().childControlWidth = false;
        cnvLastRow.GetComponent<RectTransform>().pivot = new Vector2(0, (float)0.5);
        cnvLastRow.GetComponent<RectTransform>().sizeDelta = new Vector2(cnvLastRow.transform.parent.GetComponent<RectTransform>().rect.width, Globals.CANVAS_WEB.transform.Find("cnvPacchetti/svPacchetti/Viewport/Content/cnvCell").GetComponent<RectTransform>().rect.height / 2);
    }
}
