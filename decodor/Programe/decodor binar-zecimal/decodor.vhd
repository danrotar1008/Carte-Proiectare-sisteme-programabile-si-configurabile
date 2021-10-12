----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 12.10.2021 08:43:02
-- Design Name: 
-- Module Name: decodor - Behavioral
-- Project Name: 
-- Target Devices: 
-- Tool Versions: 
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

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity decodor is
    Port ( intrare : in STD_LOGIC_VECTOR (3 downto 0);
           iesire : out STD_LOGIC_VECTOR (9 downto 0));
end decodor;

architecture Behavioral of decodor is

begin

	process(intrare)
	begin
		iesire <= "0000000000";
		case intrare is
			when "0000" => iesire(0) <= '1';
			when "0001" => iesire(1) <= '1';
			when "0010" => iesire(2) <= '1';
			when "0011" => iesire(3) <= '1';
			when "0100" => iesire(4) <= '1';
			when "0101" => iesire(5) <= '1';
			when "0110" => iesire(6) <= '1';
			when "0111" => iesire(7) <= '1';
			when "1000" => iesire(8) <= '1';
			when "1001" => iesire(9) <= '1';
			when others => iesire <= "0000000000";
		end case;
	end process;

end Behavioral;
