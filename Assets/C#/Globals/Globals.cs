using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Globals
{
    #region Objects
    #region CANVAS
    public static Canvas CANVAS_MENU;
    public static Canvas CANVAS_START;
    public static Canvas CANVAS_WEB;
    public static Canvas CANVAS_POPUP;
    #endregion
    #region MENU
    #region SCENES
    public static GameObject SCENE_MENU;
    public static GameObject SCENE_SCHEDE;
    public static GameObject SCENE_ESERCIZI;
    #endregion
    #region PAGES
    //Menu
    public static GameObject PAGE_MENU;
    public static Vector3 panelLocation;

    public static GameObject PAGE_MENU_STATS;
    public static GameObject PAGE_MENU_TRAINING;
    public static GameObject PAGE_MENU_EDIT;

    public static float X_PAGE_MENU_STATS;
    public static float X_PAGE_MENU_TRAINING;
    public static float X_PAGE_MENU_EDIT;
    //Schede
    public static GameObject PAGE_TAB_VIEW;
    //Esercizi
    public static GameObject PAGE_EXER_VIEW;
    public static GameObject PAGE_EXER_VIEW_DET;
    public static GameObject PAGE_EXER_ADD;
    public static GameObject PAGE_EXER_ADD_DET;
    #endregion
    #region TABS
    //Tabs Menu
    public static GameObject TAB_STATS;
    public static GameObject TAB_TRAINING;
    public static GameObject TAB_EDIT;
    //Tabs
    public static GameObject TAB_ANSWER;
    #endregion
    #region BUTTON
    //Answer
    public static Button BTN_CONFIRM;
    public static Button BTN_CANCEL;
    //Menu
    public static Button BTN_ADD_SCHEDA;
    public static Button BTN_ADD_ESERCIZIO;
    public static Button BTN_ADD_ESERCIZIO_DET;

    public static Button BTN_ADD_SCHEDA_ALNMT;
    public static Button BTN_ADD_ESERCIZIO_DET_ALNMT;
    public static Button BTN_ADD_ESERCIZIO_EXPL_ALNMT;
    public static Button BTN_ADD_ESERCIZIO_EXPL_REC_ALNMT;

    public static Button BTN_HOME;
    public static Button BTN_BACK;
    //Muscoli
    public static Button BTN_SPALLE;
    public static Button BTN_PETTO;
    public static Button BTN_SCHIENA;
    public static Button BTN_BRACCIA;
    public static Button BTN_ABS;
    public static Button BTN_GAMBE;
    #endregion
    #region IMAGE
    public static byte[] IMG_BYTE_INSERT;
    #endregion
    #endregion
    #region TRAINING
    #region PAGES
    public static GameObject PAGE_SCHEDE;
    public static GameObject PAGE_ESERCIZI;
    public static GameObject PAGE_ALLENAMENTO;
    public static GameObject PAGE_FINITO;

    public static Canvas CNV_ESE_REPS;
    public static Canvas CNV_ESE_TIMER;
    public static Canvas CNV_BTN_REPS;
    public static Canvas CNV_BTN_TIMER;
    public static Canvas CNV_WEIGHT;
    #endregion
    #region POPUP
    public static Canvas PP_ESERCIZIO;
    public static Canvas PP_TIME;
    public static GameObject PP_MUSCOLI;
    public static GameObject PP_BUTTON;
    #endregion
    #region BUTTON
    public static Button BTN_PAST;
    public static Button BTN_NEXT;
    public static Button BTN_THEN;
    public static Button BTN_FINITO_TIM;
    public static Button BTN_FINITO_ESE;
    #endregion
    #endregion
    #endregion

    #region Utilities
    public static int INTERO;
    public static string STATO;
    public static int POSIZIONE_ATTUALE;
    public static int POSIZIONE_FINALE;
    public static float STARTING_TIME;
    public static float CURRENT_TIME;
    public static bool BOOL;
    public static string ALLENAMENTO_MODE;
    public static int ID_ALLENAMENTO;
    public static string ES_FATTO;
    public static int COUNT_TIMER;
    public static int COUNT_FINITO;
    public static int LAST_SECONDS;
    public static int LAST_SECONDS_xRECUPERO;
    public static string TIME_RECUPERO;
    public static string LAST_ATT_DONE;
    #endregion
    public static int SQL;

    #region TABELLE
    public const string TABL_MUSCOLO = "MUSCOLO";
    public const string TABL_ME = "M_E";
    public const string TABL_ESERCIZIO = "ESERCIZIO";
    public const string TABL_SCHEDA = "SCHEDA";
    public const string TABL_ALLENAMENTO = "ALLENAMENTO";
    public const string TABL_ATTIVITA = "ATTIVITA";
    public const string TABL_DETTAGLI = "DETTAGLI_ES";
    #endregion

    #region MUSCOLO
    public const string COL_MUS_ID = "IDmuscolo";
    public const string COL_MUS_NOME = "Nome";
    #endregion
    #region M_E
    public const string COL_MxE_ID = "IDme";
    public const string COL_MxE_IDMUSCULO = "IDmuscolo";
    public const string COL_MxE_IDESERCIZIO = "IDesercizio";
    #endregion
    #region ESERCIZIO
    public const string COL_ESE_ID = "IDesercizio";
    public const string COL_ESE_NOME = "NomeEsercizio";
    public const string COL_ESE_CONSIGLI = "Consigli";
    public const string COL_ESE_NOTE = "Note";
    public const string COL_ESE_IMMAGINE = "Immagine";
    #endregion
    #region SCHEDA
    public const string COL_SCH_ID = "IDscheda";
    public const string COL_SCH_NOME = "NomeScheda";
    #endregion
    #region ALLENAMENTO
    public const string COL_ALL_ID = "IDallenamento";
    public const string COL_ALL_DATA = "Data";
    public const string COL_ALL_DURATA = "Durata";
    public const string COL_ALL_FINITO = "Finito";
    public const string COL_ALL_ID_SCHEDA = "IDscheda";
    #endregion
    #region ATTIVITA
    public const string COL_ATT_ID = "IDattivita";
    public const string COL_ATT_SERIE = "AttSerie";
    public const string COL_ATT_RIPETIZIONIn = "AttRipetizioniN";
    public const string COL_ATT_RIPETIZIONIt = "AttRipetizioniT";
    public const string COL_ATT_PESO = "AttPeso";
    public const string COL_ATT_RECUPERO = "AttRecupero";
    public const string COL_ATT_ID_DETTAGLI = "IDdettagli";
    public const string COL_ATT_ID_ALLENAMENTO = "IDallenamento";
    #endregion
    #region DETTAGLI ES
    public const string COL_DET_ID = "IDdettagli";
    public const string COL_DET_SERIE = "Serie";
    public const string COL_DET_RIPnOt = "Rip_NoT";
    public const string COL_DET_RIPETIZIONIn = "RipetizioniN";
    public const string COL_DET_RIPETIZIONIt = "RipetizioniT";
    public const string COL_DET_RECUPERO = "Recupero";
    public const string COL_DET_ATTESA = "Attesa";
    public const string COL_DET_POSIZIONE = "Posizione";
    public const string COL_DET_ID_ESERCIZIO = "IDesercizio";
    public const string COL_DET_ID_SCHEDA = "IDscheda";
    #endregion
}