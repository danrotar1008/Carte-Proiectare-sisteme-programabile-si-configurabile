library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use ieee.numeric_std.all;
 
entity demux is
	generic(N : integer:=8; M: integer:=3);
   port
   (
		in_demux: in std_logic;
		in_comanda: in std_logic_vector(M-1 downto 0);
		ies_demux: out std_logic_vector(N-1 downto 0)
   );
end entity demux;
 
architecture Behavioral of demux is
begin
	process(in_comanda, in_demux)
	variable contor: integer;
	begin
		ies_demux <= (others=>'0');
		contor := to_integer(unsigned(in_comanda));
		ies_demux(contor)<=in_demux;
	end process;
end architecture Behavioral;





