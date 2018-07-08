#include <stdio.h>

int main () {
	int num;
	int FATT;
	
	printf("Inserire il numero di cui calcolare il fattoriale:");
	scanf("%d", &num);
	printf("\n");
	
	FATT=1;
	while(num!=1){
		FATT=FATT*num;
		num--;
	}
	
	printf("Il fattoriale di %d e' %d", num,FATT);
	scanf("%d", &num);
}
