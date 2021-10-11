--------------------------------------------------------------------------------
-- Company: 
-- Engineer:
--
-- Create Date:   11:38:31 10/06/2021
-- Design Name:   
-- Module Name:   C:/LUCRU/Poarta_AND/Poarta_AND_TB.vhd
-- Project Name:  Poarta_AND
-- Target Device:  
-- Tool versions:  
-- Description:   
-- 
-- VHDL Test Bench Created by ISE for module: Poarta_AND
-- 
-- Dependencies:
-- 
-- Revision:
-- Revision 0.01 - File Created
-- Additional Comments:
--
-- Notes: 
-- This testbench has been automatically generated using types std_logic and
-- std_logic_vector for the ports of the unit under test.  Xilinx recommends
-- that these types always be used for the top-level I/O of a design in order
-- to guarantee that the testbench will bind correctly to the post-implementation 
-- simulation model.
--------------------------------------------------------------------------------
LIBRARY ieee;
USE ieee.std_logic_1164.ALL;
 
-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--USE ieee.numeric_std.ALL;
 
ENTITY Poarta_AND_TB IS
END Poarta_AND_TB;
 
ARCHITECTURE behavior OF Poarta_AND_TB IS 
 
    -- Component Declaration for the Unit Under Test (UUT)
 
    COMPONENT Poarta_AND
    PORT(
         In0 : IN  std_logic;
         In1 : IN  std_logic;
         Out0 : OUT  std_logic
        );
    END COMPONENT;
    

   --Inputs
   signal In0 : std_logic := '0';
   signal In1 : std_logic := '0';

 	--Outputs
   signal Out0 : std_logic;
   -- No clocks detected in port list. Replace <clock> below with 
   -- appropriate port name 
 
   -- constant <clock>_period : time := 10 ns;
 
BEGIN
 
	-- Instantiate the Unit Under Test (UUT)
   uut: Poarta_AND PORT MAP (
          In0 => In0,
          In1 => In1,
          Out0 => Out0
        );

   -- Clock process definitions
--   <clock>_process :process
--   begin
--		<clock> <= '0';
--		wait for <clock>_period/2;
--		<clock> <= '1';
--		wait for <clock>_period/2;
--   end process;
 

   -- Stimulus process
   stim_proc: process
   begin		
      -- hold reset state for 100 ns.
      wait for 100 ns;	

      -- wait for <clock>_period*10;

      -- insert stimulus here 
		In1 <= '0';
		In0 <= '0';

		wait for 100 ns;
		In1 <= '0';
		In0 <= '1';

		wait for 100 ns;
		In1 <= '1';
		In0 <= '0';

		wait for 100 ns;
		In1 <= '1';
		In0 <= '1';

		wait for 100 ns;
		In1 <= '0';
		In0 <= '0';

      wait;
   end process;

END;
