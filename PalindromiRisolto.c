#include<stdio.h>
#define N 10

/*
Proposta di formulazione alternativa: 
Scrivi una funzione che genera un array di 0 ed 1 di lunghezza N e verifica se è un simmetrico. 
Scrivi un programma che, di continuo, genera array e poi li ispeziona, interrompendosi non appena trova un array simmetrico e conta quanti tentativi vanno fatti prima di trovarne uno. Che relazione c'è tra N ed il numero di tentativi? 
*/


int palindromoN1(int* V,int lenght) {
	int i;
	for (i=0;i<lenght/2;i++)
		if(V[i]!=V[lenght-i-1])
			return 0;
	return 1;
	//Non appena la funzione trova una coppia di valori diversa, si esaurisce e lascia come valore di ritorno 0
}

int palindromoN2(char* V, int lenght) {
	int i;
	int count;
	char A[lenght];
	count=0;
	for (i=0;i<lenght;i++)
		if(V[i]!=' '&&V[i]!=0){
			A[count]=V[i];
			count++;
		}
	for(i=0;i<lenght;i++)
		printf("%c",A[i]);
	printf("\n");
	//A questo punto del programma, avremo un array A=[c,s,s,s,s,c,0,0,0,0] ed un count che si ricorda quanto è grande il nuovo array
	for (i=0;i<count/2+1;i++)
		if(A[i]!=A[count-i-1])
			return 0;
	return 1;
}

int main () {
	int A[N]={2,0,0,1,0,1,0,2,0,0};
	int B[N]={1,2,0,5,7,7,5,0,2,1};
	char C[N]={'c','s',' ','s',' ',' ','s','s','c',' '};
	int i;
	int x,y;
	

	x= palindromoN1(B,N);
	y= palindromoN2(C,N);
	
	
	for(i=0;i<10;i++)
		printf("%d",A[i]);
	printf("\n");
	for(i=0;i<10;i++)
		printf("%d",B[i]);
	printf("\n");
	for(i=0;i<10;i++)
		printf("%c",C[i]);
	printf("\n");
	//Stampa dei 3 vettori
	
	
	if(x==1)
		printf("La parola e' un palindromo!\n");
	else
		printf("La parola non e' un palindromo\n");
	//Verifica che l'array A contiene un palindromo
	
	if(y==1)
		printf("La parola e' un palindromo (a meno degli spazi)!\n");
	else
		printf("La parola non e' un palindromo (a meno degli spazi)!\n");
	return 0;	
	//Verifica che l'array C, a meno degli spazi, contiene un palindromo
	scanf("%d",&A[0]);
}
