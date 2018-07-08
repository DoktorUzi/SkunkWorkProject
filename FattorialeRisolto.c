#include <stdio.h>

int main () {
	int num;
	int FATTONE;
	
	printf("Inserire il numero di cui calcolare il Fattoriale:");
	scanf("%d", &num);
	printf("\n");
	
	FATTONE=1;
	while(num!=1){
		FATTONE=FATTONE*num;
		num--;
	}
	
	printf("Il Fattoriale di %d e' %d", num,FATTONE);
	scanf("%d", &num);
}
