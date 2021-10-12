library ieee;
use ieee.std_logic_1164.all;
use ieee.numeric_std.all;

entity demux_tb is
end demux_tb;

architecture arch_demux_tb of demux_tb is
signal test_intrare: std_logic;
signal test_comanda: std_logic_vector(1 downto 0);
signal test_iesire: std_logic_vector(3 downto 0);

begin
uut: entity dmux.demux(behavioral)
	generic map(N => 4, M => 2)
	port map(in_demux => test_intrare, in_comanda => test_comanda, ies_demux => test_iesire);

-- generarea vectorilor de test
	process
	begin
		test_intrare <= '0';
		test_comanda <= "00";
		test_iesire <= "0000";
		wait for 100 ns;
		loop -- bucla infinita
			test_intrare <= '0';
			wait for 100 ns;
			test_intrare <= '1';
			wait for 100 ns;
			test_comanda <= std_logic_vector(unsigned(test_comanda) + "1");
			wait for 100 ns;
		end loop;
	end process;
end arch_demux_tb;




