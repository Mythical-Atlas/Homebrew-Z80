useful	equ 45

section .text
_start:							;tell linker entry point
	ld b, 7
	ld c, 3
	call _multiply_bytes_start	;call multiplication subroutine
	nop
	nop
	nop
	nop
	nop
	nop
	nop
	nop
	nop
	nop
	
	
	
	
	
_multiply_bytes_start:			;bytes meant to be multiplied must be in b and c
	ld a, 0						;reset product to 0
_multiply_bytes_loop:			;loop here if multiplication is not complete
	add a, b
	dec c						;decement counter
	jp nz, _multiply_bytes_loop	;loop if multiplication not complete
	ret							;return from subroutine

section .data
choice		db 0
application	db 0