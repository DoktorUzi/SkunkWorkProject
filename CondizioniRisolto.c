#include<stdio.h>
#define N 5

// Meno patate più documents

int seprimo (int num) {
	int count=0;
	while (count <= num/2) {
		if(num%count==0)
			return 0;
		count++;
	}
	return 1;
}

int main () {
	int A[N][N]={-2,13,-5,8,3,-5,1,8,3,-18,0,0,3,7,-3,2,5,13,15,6,20,7,3,12,3};
	int i,j;
	for(i=0;i<N;i++){
		for(j=0;j<N;j++)
			printf("\t%d", A[i][j]);
		printf("\n\n");
	}
	
	/*
	Scrivere un segmento di codice che, data la matrice A, stampa a schermo i numeri che soddisfano una delle seguenti condizioni:
	-negativo, in posizione dispari
	-positivo, divisibile per 3 e multiplo di 2
	N.B.: si dice elemento in posizione dispari un elemento per cui (-1)^x=-1 con x=i+j, dove i è il numero di riga e j è il numero di colonna (es: l'elemento in riga 2 colonna 3 è dispari, in quanto (-1)^5=-1, l'elemento in riga 3 colonna 5 è pari, in quanto (-1)^8=1)
	
	Provare a scrivere come una catena di IF-ELSE e/o come un unica condizione di un IF-ELSE
	
	Esercizio supplementare: Si aggiunga ad entrambe le condizioni precedenti il requisito che il numero sia PRIMO (sarà necessario usare la sottofunzione seprimo())
	*/
	
	printf("\nElenco dei numeri che soddisfano la prima condizione:\n");
	for(i=0;i<N;i++)
		for(j=0;j<N;j++)
			if ((A[i][j]<0 && (i+j)%2==1) || (A[i][j]>0 && A[i][j]%3==0 && A[i][j]%2==0))
				printf(" %d", A[i][j]);
	printf("\nElenco dei numeri che soddisfano la seconda condizione:\n");
	for(i=0;i<N;i++)
		for(j=0;j<N;j++)
			if (((A[i][j]<0 && (i+j)%2==1) || (A[i][j]>0 && A[i][j]%3==0 && A[i][j]%2==0)) && seprimo(A[i][j]))
				printf(" %d", A[i][j]);
	printf("\n\n");
	system("PAUSE");
}
