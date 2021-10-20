----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    19:24:50 01/06/2015 
-- Design Name: 
-- Module Name:    modul_7seg - Behavioral 
-- Project Name: 
-- Target Devices: 
-- Tool versions: 
-- Description: 
--
-- Dependencies: 
--
-- Revision: 
-- Revision 0.01 - File Created
-- Additional Comments: 
--
----------------------------------------------------------------------------------
library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use ieee.std_logic_unsigned.all; 

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx primitives in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity modul_7seg is
    Port ( clk : in  STD_LOGIC;
           port_in : in  STD_LOGIC_VECTOR (3 downto 0);
           nr_port : in  STD_LOGIC_VECTOR (1 downto 0);
           anod : out  STD_LOGIC_VECTOR (3 downto 0);
           segment : out  STD_LOGIC_VECTOR (7 downto 0));
end modul_7seg;

architecture Behavioral of modul_7seg is

constant N : integer := 15; -- dimensiunea registrului de divizare ceas

signal contor: std_logic_vector (N downto 0) := (others => '0');
signal next_contor: std_logic_vector(N downto 0);

signal zeci_ore: std_logic_vector(3 downto 0);
signal next_zeci_ore: std_logic_vector(3 downto 0);
signal ore: std_logic_vector(3 downto 0);
signal next_ore: std_logic_vector(3 downto 0);
signal zeci_minute: std_logic_vector(3 downto 0);
signal next_zeci_minute: std_logic_vector(3 downto 0);
signal minute: std_logic_vector(3 downto 0);
signal next_minute: std_logic_vector(3 downto 0);
-- -------------------
signal sel_cifra: std_logic_vector(1 downto 0);
signal cifra_decodificata: std_logic_vector(3 downto 0);
signal seven_seg: std_logic_vector(6 downto 0);

begin

process (clk, port_in, nr_port, next_zeci_ore, 
next_ore, next_zeci_minute, next_minute, sel_cifra,
minute, zeci_minute, ore, zeci_ore)
begin
	if (clk'event and clk = '1') then
		next_contor <= next_contor + 1;

		case nr_port is
			when "11" => -- nr port = 3
				next_zeci_ore <= port_in;
			when "10" => -- nr port = 2
				next_ore <= port_in;
			when "01" => -- nr port = 1
				next_zeci_minute <= port_in;
			when others => -- nr port = 0
				next_minute <= port_in;
		end case;

	end if;

		zeci_ore <= next_zeci_ore;
		ore <= next_ore;
		zeci_minute <= next_zeci_minute;
		minute <= next_minute;
	
	case sel_cifra is
		when "00" =>
			cifra_decodificata <= minute;
			anod <= "1110";
		when "01" =>
			cifra_decodificata <= zeci_minute;
			anod <= "1101";
		when "10" =>
			cifra_decodificata <= ore;
			anod <= "1011";
		when others =>
			cifra_decodificata <= zeci_ore;
			anod <= "0111";
		end case;
end process;

contor <= next_contor;
sel_cifra <= contor(N downto N-1);

-- iesire
	
	with cifra_decodificata(3 downto 0) select
		seven_seg <=
			"0000001" when "0000",
			"1001111" when "0001",
			"0010010" when "0010",
			"0000110" when "0011",
			"1001100" when "0100",
			"0100100" when "0101",
			"0100000" when "0110",
			"0001111" when "0111",
			"0000000" when "1000",
			"0000100" when "1001",
			"0001000" when "1010", --a
			"1100000" when "1011", --b
			"0110001" when "1100", --c
			"1000010" when "1101", --d
			"0110000" when "1110", --e
			"0111000" when "1111", --f
			"0000000" when others;

	segment(6 downto 0) <= seven_seg;
	segment(7) <= '1';

end Behavioral;

