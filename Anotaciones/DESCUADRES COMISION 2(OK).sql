--SOBRE EL BRUTO NO DEBERIA HABER DESCUADRES DESPUES DE FACTURAS EMITIDAS EN MARZO

SELECT FACT_DAEM,DECODE(FACT_ANUL,'0','EMITIDA','1','CANCELADA'),TNHT_DEFA.SEFA_CODI , TNHT_DEFA.FACT_CODI,SUM(DEFA_VALO),SUM(DEFA_VLIQ),SUM(DEFA_IMP1),SUM(DEFA_BRUT) , ( SUM(DEFA_VLIQ) + SUM(DEFA_IMP1)),  
(SUM(DEFA_BRUT)- ( SUM(DEFA_VLIQ) + SUM(DEFA_IMP1)) ) as DESCUADRE,DECODE(TNHT_DESF.DESF_TIDE,'0','BRUTO','1','NETO')

FROM RMCGHC.TNHT_DEFA,RMCGHC.TNHT_DESF,RMCGHC.TNHT_FACT

WHERE TNHT_DEFA.SEFA_CODI = TNHT_DESF.SEFA_CODI AND  TNHT_DEFA.FACT_CODI = TNHT_DESF.FACT_CODI
AND      TNHT_DEFA.SEFA_CODI = TNHT_FACT.SEFA_CODI AND  TNHT_DEFA.FACT_CODI = TNHT_FACT.FACT_CODI
 
GROUP BY TNHT_DEFA.SEFA_CODI , TNHT_DEFA.FACT_CODI,TNHT_DESF.DESF_TIDE,FACT_DAEM,FACT_ANUL
HAVING round(SUM(DEFA_BRUT),2) <>  round(( SUM(DEFA_VLIQ) + SUM(DEFA_IMP1)),2) 
order by sefa_codi,fact_codi